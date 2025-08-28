using MediatR;
using Backendapiservice.Application.DTOs;

namespace Backendapiservice.Application.Commands
{
    public record CreateAssistantCommand(CreateAssistantDto AssistantData) : IRequest<AssistantDto>;

    public record UpdateAssistantCommand(int Id, CreateAssistantDto AssistantData) : IRequest<AssistantDto?>;
}