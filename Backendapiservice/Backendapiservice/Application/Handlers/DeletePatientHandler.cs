using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class DeletePatientHandler : IRequestHandler<DeletePatientCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeletePatientHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (patient == null)
                return false;

            // Business rule: Cannot delete patient with scheduled appointments
            var scheduledAppointments = patient.Appointments.Where(a => a.Status == "Scheduled").ToList();
            if (scheduledAppointments.Any())
            {
                throw new InvalidOperationException($"Cannot delete patient with {scheduledAppointments.Count} scheduled appointments.");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}