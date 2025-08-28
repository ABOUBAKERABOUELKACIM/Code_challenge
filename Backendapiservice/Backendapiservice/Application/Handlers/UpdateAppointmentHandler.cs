using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, AppointmentDto?>
    {
        private readonly AppDbContext _context;

        public UpdateAppointmentHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDto?> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (appointment == null)
                return null;

            appointment.AppointmentDateTime = request.AppointmentData.AppointmentDateTime;
            appointment.Notes = request.AppointmentData.Notes;
            appointment.DoctorId = request.AppointmentData.DoctorId;
            appointment.PatientId = request.AppointmentData.PatientId;
            appointment.CreatedByAssistantId = request.AppointmentData.CreatedByAssistantId;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            // Reload to get updated relationships
            await _context.Entry(appointment).ReloadAsync(cancellationToken);
            await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync(cancellationToken);
            await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync(cancellationToken);
            await _context.Entry(appointment).Reference(a => a.CreatedByAssistant).LoadAsync(cancellationToken);

            return new AppointmentDto
            {
                Id = appointment.Id,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.FullName,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName,
                CreatedByAssistantId = appointment.CreatedByAssistantId,
                CreatedByAssistantName = appointment.CreatedByAssistant?.FullName,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }
    }

    public class UpdateAppointmentStatusHandler : IRequestHandler<UpdateAppointmentStatusCommand, AppointmentDto?>
    {
        private readonly AppDbContext _context;

        public UpdateAppointmentStatusHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDto?> Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (appointment == null)
                return null;

            appointment.Status = request.StatusData.Status;
            if (!string.IsNullOrEmpty(request.StatusData.Notes))
            {
                appointment.Notes = request.StatusData.Notes;
            }
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new AppointmentDto
            {
                Id = appointment.Id,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.FullName,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName,
                CreatedByAssistantId = appointment.CreatedByAssistantId,
                CreatedByAssistantName = appointment.CreatedByAssistant?.FullName,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }
    }
}