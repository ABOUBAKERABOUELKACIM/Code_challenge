using MediatR;
using Backendapiservice.Application.Queries;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class GetDoctorsHandler : IRequestHandler<GetDoctorsQuery, List<DoctorDto>>
    {
        private readonly AppDbContext _context;

        public GetDoctorsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDto>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Doctors
                .Include(d => d.Office)
                .Include(d => d.Assistants)
                .Include(d => d.Appointments)
                .Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization,
                    Email = d.Email,
                    OfficeId = d.OfficeId,
                    OfficeName = d.Office.Name,
                    CreatedAt = d.CreatedAt,
                    AssistantsCount = d.Assistants.Count,
                    AppointmentsCount = d.Appointments.Count
                })
                .ToListAsync(cancellationToken);
        }
    }

    public class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto?>
    {
        private readonly AppDbContext _context;

        public GetDoctorByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorDto?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Doctors
                .Include(d => d.Office)
                .Include(d => d.Assistants)
                .Include(d => d.Appointments)
                .Where(d => d.Id == request.Id)
                .Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization,
                    Email = d.Email,
                    OfficeId = d.OfficeId,
                    OfficeName = d.Office.Name,
                    CreatedAt = d.CreatedAt,
                    AssistantsCount = d.Assistants.Count,
                    AppointmentsCount = d.Appointments.Count
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    public class GetDoctorsByOfficeHandler : IRequestHandler<GetDoctorsByOfficeQuery, List<DoctorDto>>
    {
        private readonly AppDbContext _context;

        public GetDoctorsByOfficeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDto>> Handle(GetDoctorsByOfficeQuery request, CancellationToken cancellationToken)
        {
            return await _context.Doctors
                .Include(d => d.Office)
                .Include(d => d.Assistants)
                .Include(d => d.Appointments)
                .Where(d => d.OfficeId == request.OfficeId)
                .Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization,
                    Email = d.Email,
                    OfficeId = d.OfficeId,
                    OfficeName = d.Office.Name,
                    CreatedAt = d.CreatedAt,
                    AssistantsCount = d.Assistants.Count,
                    AppointmentsCount = d.Appointments.Count
                })
                .ToListAsync(cancellationToken);
        }
    }
}