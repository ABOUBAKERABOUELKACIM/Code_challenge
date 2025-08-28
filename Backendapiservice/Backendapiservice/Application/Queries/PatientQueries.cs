using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Queries
{
    public record GetPatientsQuery : IRequest<List<PatientDto>>;

    public record GetPatientByIdQuery(int Id) : IRequest<PatientDto?>;
}