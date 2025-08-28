using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, OfficeDto?>
    {
        private readonly AppDbContext _context;

        public UpdateOfficeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OfficeDto?> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _context.Offices
                .Include(o => o.Doctors)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (office == null)
                return null;

            office.Name = request.OfficeData.Name;
            office.Address = request.OfficeData.Address;
            office.Phone = request.OfficeData.Phone;

            await _context.SaveChangesAsync(cancellationToken);

            return new OfficeDto
            {
                Id = office.Id,
                Name = office.Name,
                Address = office.Address,
                Phone = office.Phone,
                CreatedAt = office.CreatedAt,
                DoctorsCount = office.Doctors.Count
            };
        }
    }
}