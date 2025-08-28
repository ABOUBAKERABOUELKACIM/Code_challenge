using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record CreatePatientCommand(CreatePatientDto PatientData) : IRequest<PatientDto>;

    public record UpdatePatientCommand(int Id, CreatePatientDto PatientData) : IRequest<PatientDto?>;
}