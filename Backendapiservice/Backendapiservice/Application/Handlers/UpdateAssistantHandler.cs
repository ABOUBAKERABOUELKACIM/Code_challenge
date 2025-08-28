using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class UpdateAssistantHandler : IRequestHandler<UpdateAssistantCommand, AssistantDto?>
    {
        private readonly AppDbContext _context;

        public UpdateAssistantHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssistantDto?> Handle(UpdateAssistantCommand request, CancellationToken cancellationToken)
        {
            var assistant = await _context.Assistants
                .Include(a => a.Doctor)
                .Include(a => a.CreatedAppointments)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (assistant == null)
                return null;

            assistant.FirstName = request.AssistantData.FirstName;
            assistant.LastName = request.AssistantData.LastName;
            assistant.Email = request.AssistantData.Email;
            assistant.DoctorId = request.AssistantData.DoctorId;

            await _context.SaveChangesAsync(cancellationToken);

            // Reload doctor if changed
            if (assistant.DoctorId != assistant.Doctor.Id)
            {
                await _context.Entry(assistant).Reference(a => a.Doctor).LoadAsync(cancellationToken);
            }

            return new AssistantDto
            {
                Id = assistant.Id,
                FirstName = assistant.FirstName,
                LastName = assistant.LastName,
                Email = assistant.Email,
                DoctorId = assistant.DoctorId,
                DoctorName = assistant.Doctor.FullName,
                CreatedAt = assistant.CreatedAt,
                CreatedAppointmentsCount = assistant.CreatedAppointments.Count
            };
        }
    }
}