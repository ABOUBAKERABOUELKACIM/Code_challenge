using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Domain.Entities;
using Backendapiservice.Domain.Events;
using Backendapiservice.Domain.Interfaces;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
    {
        private readonly AppDbContext _context;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CreateAppointmentHandler(AppDbContext context, IDomainEventPublisher domainEventPublisher)
        {
            _context = context;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task<AppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = new Appointment
            {
                AppointmentDateTime = request.AppointmentData.AppointmentDateTime,
                Status = "Scheduled",
                Notes = request.AppointmentData.Notes,
                DoctorId = request.AppointmentData.DoctorId,
                PatientId = request.AppointmentData.PatientId,
                CreatedByAssistantId = request.AppointmentData.CreatedByAssistantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            // Publish domain event
            var domainEvent = new AppointmentCreatedEvent(
                appointment.Id,
                appointment.DoctorId,
                appointment.PatientId,
                appointment.AppointmentDateTime);

            await _domainEventPublisher.PublishAsync(domainEvent);

            // Load related entities for response
            var createdAppointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.CreatedByAssistant)
                .FirstAsync(a => a.Id == appointment.Id, cancellationToken);

            return new AppointmentDto
            {
                Id = createdAppointment.Id,
                AppointmentDateTime = createdAppointment.AppointmentDateTime,
                Status = createdAppointment.Status,
                Notes = createdAppointment.Notes,
                DoctorId = createdAppointment.DoctorId,
                DoctorName = createdAppointment.Doctor.FullName,
                PatientId = createdAppointment.PatientId,
                PatientName = createdAppointment.Patient.FullName,
                CreatedByAssistantId = createdAppointment.CreatedByAssistantId,
                CreatedByAssistantName = createdAppointment.CreatedByAssistant?.FullName,
                CreatedAt = createdAppointment.CreatedAt,
                UpdatedAt = createdAppointment.UpdatedAt
            };
        }
    }
}