using MediatR;
using Backendapiservice.Application.Queries;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;  // <-- This should match your AppDbContext namespace
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Application.Handlers
{
    public class GetOfficesHandler : IRequestHandler<GetOfficesQuery, List<OfficeDto>>
    {
        private readonly AppDbContext _context;

        public GetOfficesHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OfficeDto>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Offices
                .Include(o => o.Doctors)
                .Select(o => new OfficeDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address,
                    Phone = o.Phone,
                    CreatedAt = o.CreatedAt,
                    DoctorsCount = o.Doctors.Count
                })
                .ToListAsync(cancellationToken);
        }
    }

    public class GetOfficeByIdHandler : IRequestHandler<GetOfficeByIdQuery, OfficeDto?>
    {
        private readonly AppDbContext _context;

        public GetOfficeByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OfficeDto?> Handle(GetOfficeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Offices
                .Include(o => o.Doctors)
                .Where(o => o.Id == request.Id)
                .Select(o => new OfficeDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address,
                    Phone = o.Phone,
                    CreatedAt = o.CreatedAt,
                    DoctorsCount = o.Doctors.Count
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}