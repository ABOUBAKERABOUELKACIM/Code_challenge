using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Domain.Entities;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class CreateAssistantHandler : IRequestHandler<CreateAssistantCommand, AssistantDto>
    {
        private readonly AppDbContext _context;

        public CreateAssistantHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssistantDto> Handle(CreateAssistantCommand request, CancellationToken cancellationToken)
        {
            var assistant = new Assistant
            {
                FirstName = request.AssistantData.FirstName,
                LastName = request.AssistantData.LastName,
                Email = request.AssistantData.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.AssistantData.Password),
                DoctorId = request.AssistantData.DoctorId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Assistants.Add(assistant);
            await _context.SaveChangesAsync(cancellationToken);

            // Load doctor name for response
            var doctor = await _context.Doctors.FindAsync(assistant.DoctorId);

            return new AssistantDto
            {
                Id = assistant.Id,
                FirstName = assistant.FirstName,
                LastName = assistant.LastName,
                Email = assistant.Email,
                DoctorId = assistant.DoctorId,
                DoctorName = doctor?.FullName ?? "",
                CreatedAt = assistant.CreatedAt,
                CreatedAppointmentsCount = 0
            };
        }
    }
}