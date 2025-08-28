using MediatR;
using Backendapiservice.Application.Queries;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class GetPatientsHandler : IRequestHandler<GetPatientsQuery, List<PatientDto>>
    {
        private readonly AppDbContext _context;

        public GetPatientsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PatientDto>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    Phone = p.Phone,
                    CreatedAt = p.CreatedAt,
                    AppointmentsCount = p.Appointments.Count
                })
                .ToListAsync(cancellationToken);
        }
    }

    public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, PatientDto?>
    {
        private readonly AppDbContext _context;

        public GetPatientByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PatientDto?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .Where(p => p.Id == request.Id)
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    Phone = p.Phone,
                    CreatedAt = p.CreatedAt,
                    AppointmentsCount = p.Appointments.Count
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}