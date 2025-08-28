using MediatR;
using Backendapiservice.Application.Commands;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Backendapiservice.Infrastructure.Data;

namespace Backendapiservice.Application.Handlers
{
    public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, OfficeDto>
    {
        private readonly AppDbContext _context;

        public CreateOfficeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OfficeDto> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = new Office
            {
                Name = request.OfficeData.Name,
                Address = request.OfficeData.Address,
                Phone = request.OfficeData.Phone,
                CreatedAt = DateTime.UtcNow
            };

            _context.Offices.Add(office);
            await _context.SaveChangesAsync(cancellationToken);

            return new OfficeDto
            {
                Id = office.Id,
                Name = office.Name,
                Address = office.Address,
                Phone = office.Phone,
                CreatedAt = office.CreatedAt,
                DoctorsCount = 0
            };
        }
    }
}