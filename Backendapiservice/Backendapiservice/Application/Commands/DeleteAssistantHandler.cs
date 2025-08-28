using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class DeleteAssistantHandler : IRequestHandler<DeleteAssistantCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteAssistantHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteAssistantCommand request, CancellationToken cancellationToken)
        {
            var assistant = await _context.Assistants
                .Include(a => a.CreatedAppointments)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (assistant == null)
                return false;

            // Set CreatedByAssistantId to null for all appointments created by this assistant
            foreach (var appointment in assistant.CreatedAppointments)
            {
                appointment.CreatedByAssistantId = null;
            }

            _context.Assistants.Remove(assistant);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}