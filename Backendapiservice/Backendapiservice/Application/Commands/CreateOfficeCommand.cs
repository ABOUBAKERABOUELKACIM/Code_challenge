using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record CreateOfficeCommand(CreateOfficeDto OfficeData) : IRequest<OfficeDto>;
}