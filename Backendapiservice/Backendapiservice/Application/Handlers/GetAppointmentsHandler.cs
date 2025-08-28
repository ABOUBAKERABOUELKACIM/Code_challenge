using MediatR;
using Backendapiservice.Application.Queries;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;

        public GetAppointmentsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    CreatedByAssistantId = a.CreatedByAssistantId,
                    CreatedByAssistantName = a.CreatedByAssistant != null ? a.CreatedByAssistant.FullName : null,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync(cancellationToken);
        }
    }

    public class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
    {
        private readonly AppDbContext _context;

        public GetAppointmentByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .Where(a => a.Id == request.Id)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    CreatedByAssistantId = a.CreatedByAssistantId,
                    CreatedByAssistantName = a.CreatedByAssistant != null ? a.CreatedByAssistant.FullName : null,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    public class GetAppointmentsByDoctorHandler : IRequestHandler<GetAppointmentsByDoctorQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;

        public GetAppointmentsByDoctorHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByDoctorQuery request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .Where(a => a.DoctorId == request.DoctorId)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    CreatedByAssistantId = a.CreatedByAssistantId,
                    CreatedByAssistantName = a.CreatedByAssistant != null ? a.CreatedByAssistant.FullName : null,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync(cancellationToken);
        }
    }

    public class GetAppointmentsByPatientHandler : IRequestHandler<GetAppointmentsByPatientQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;

        public GetAppointmentsByPatientHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByPatientQuery request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .Where(a => a.PatientId == request.PatientId)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    CreatedByAssistantId = a.CreatedByAssistantId,
                    CreatedByAssistantName = a.CreatedByAssistant != null ? a.CreatedByAssistant.FullName : null,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync(cancellationToken);
        }
    }

    public class GetAppointmentsByDateRangeHandler : IRequestHandler<GetAppointmentsByDateRangeQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;

        public GetAppointmentsByDateRangeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .Where(a => a.AppointmentDateTime >= request.StartDate && a.AppointmentDateTime <= request.EndDate)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    CreatedByAssistantId = a.CreatedByAssistantId,
                    CreatedByAssistantName = a.CreatedByAssistant != null ? a.CreatedByAssistant.FullName : null,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync(cancellationToken);
        }
    }
}