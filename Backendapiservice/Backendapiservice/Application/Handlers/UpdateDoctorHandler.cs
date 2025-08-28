using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand, DoctorDto?>
    {
        private readonly AppDbContext _context;

        public UpdateDoctorHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorDto?> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Office)
                .Include(d => d.Assistants)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (doctor == null)
                return null;

            doctor.FirstName = request.DoctorData.FirstName;
            doctor.LastName = request.DoctorData.LastName;
            doctor.Specialization = request.DoctorData.Specialization;
            doctor.Email = request.DoctorData.Email;
            doctor.OfficeId = request.DoctorData.OfficeId;

            await _context.SaveChangesAsync(cancellationToken);

            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                Email = doctor.Email,
                OfficeId = doctor.OfficeId,
                OfficeName = doctor.Office.Name,
                CreatedAt = doctor.CreatedAt,
                AssistantsCount = doctor.Assistants.Count,
                AppointmentsCount = doctor.Appointments.Count
            };
        }
    }
}