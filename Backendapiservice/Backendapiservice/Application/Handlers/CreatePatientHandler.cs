using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Domain.Entities;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, PatientDto>
    {
        private readonly AppDbContext _context;

        public CreatePatientHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient
            {
                FirstName = request.PatientData.FirstName,
                LastName = request.PatientData.LastName,
                Email = request.PatientData.Email,
                Phone = request.PatientData.Phone,
                CreatedAt = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync(cancellationToken);

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                CreatedAt = patient.CreatedAt,
                AppointmentsCount = 0
            };
        }
    }
}