using MediatR;
using Backendapiservice.Application.Queries;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class GetAssistantsHandler : IRequestHandler<GetAssistantsQuery, List<AssistantDto>>
    {
        private readonly AppDbContext _context;

        public GetAssistantsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssistantDto>> Handle(GetAssistantsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Assistants
                .Include(a => a.Doctor)
                .Include(a => a.CreatedAppointments)
                .Select(a => new AssistantDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    CreatedAt = a.CreatedAt,
                    CreatedAppointmentsCount = a.CreatedAppointments.Count
                })
                .ToListAsync(cancellationToken);
        }
    }

    public class GetAssistantByIdHandler : IRequestHandler<GetAssistantByIdQuery, AssistantDto?>
    {
        private readonly AppDbContext _context;

        public GetAssistantByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssistantDto?> Handle(GetAssistantByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Assistants
                .Include(a => a.Doctor)
                .Include(a => a.CreatedAppointments)
                .Where(a => a.Id == request.Id)
                .Select(a => new AssistantDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    CreatedAt = a.CreatedAt,
                    CreatedAppointmentsCount = a.CreatedAppointments.Count
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    public class GetAssistantsByDoctorHandler : IRequestHandler<GetAssistantsByDoctorQuery, List<AssistantDto>>
    {
        private readonly AppDbContext _context;

        public GetAssistantsByDoctorHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssistantDto>> Handle(GetAssistantsByDoctorQuery request, CancellationToken cancellationToken)
        {
            return await _context.Assistants
                .Include(a => a.Doctor)
                .Include(a => a.CreatedAppointments)
                .Where(a => a.DoctorId == request.DoctorId)
                .Select(a => new AssistantDto
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    CreatedAt = a.CreatedAt,
                    CreatedAppointmentsCount = a.CreatedAppointments.Count
                })
                .ToListAsync(cancellationToken);
        }
    }
}