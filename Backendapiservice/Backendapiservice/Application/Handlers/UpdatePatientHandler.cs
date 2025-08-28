using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand, PatientDto?>
    {
        private readonly AppDbContext _context;

        public UpdatePatientHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PatientDto?> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (patient == null)
                return null;

            patient.FirstName = request.PatientData.FirstName;
            patient.LastName = request.PatientData.LastName;
            patient.Email = request.PatientData.Email;
            patient.Phone = request.PatientData.Phone;

            await _context.SaveChangesAsync(cancellationToken);

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                CreatedAt = patient.CreatedAt,
                AppointmentsCount = patient.Appointments.Count
            };
        }
    }
}