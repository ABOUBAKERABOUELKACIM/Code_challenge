using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record UpdateOfficeCommand(int Id, CreateOfficeDto OfficeData) : IRequest<OfficeDto?>;
}