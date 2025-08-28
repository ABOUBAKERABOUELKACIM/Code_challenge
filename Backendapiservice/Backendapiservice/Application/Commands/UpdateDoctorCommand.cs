using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record UpdateDoctorCommand(int Id, CreateDoctorDto DoctorData) : IRequest<DoctorDto?>;
}