using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record CreateDoctorCommand(CreateDoctorDto DoctorData) : IRequest<DoctorDto>;
}