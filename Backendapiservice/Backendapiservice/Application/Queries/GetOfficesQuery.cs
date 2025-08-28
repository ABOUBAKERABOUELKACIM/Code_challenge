using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Queries
{
    public record GetOfficesQuery : IRequest<List<OfficeDto>>;

    public record GetOfficeByIdQuery(int Id) : IRequest<OfficeDto?>;
}