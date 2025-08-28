using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Queries
{
    public record GetAssistantsQuery : IRequest<List<AssistantDto>>;

    public record GetAssistantByIdQuery(int Id) : IRequest<AssistantDto?>;

    public record GetAssistantsByDoctorQuery(int DoctorId) : IRequest<List<AssistantDto>>;
}