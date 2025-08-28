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
    public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
    {
        private readonly AppDbContext _context;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CreateDoctorHandler(AppDbContext context, IDomainEventPublisher domainEventPublisher)
        {
            _context = context;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task<DoctorDto> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = new Doctor
            {
                FirstName = request.DoctorData.FirstName,
                LastName = request.DoctorData.LastName,
                Specialization = request.DoctorData.Specialization,
                Email = request.DoctorData.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.DoctorData.Password),
                OfficeId = request.DoctorData.OfficeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            // Load office for event and response
            var office = await _context.Offices.FindAsync(doctor.OfficeId);

            // Publish domain event
            var domainEvent = new DoctorAddedToOfficeEvent(
                doctor.Id,
                doctor.OfficeId,
                doctor.FullName,
                office?.Name ?? "Unknown Office");

            await _domainEventPublisher.PublishAsync(domainEvent);

            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                Email = doctor.Email,
                OfficeId = doctor.OfficeId,
                OfficeName = office?.Name ?? "",
                CreatedAt = doctor.CreatedAt,
                AssistantsCount = 0,
                AppointmentsCount = 0
            };
        }
    }
}