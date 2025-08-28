using Backendapiservice.Domain.Events;

namespace Backendapiservice.Domain.Interfaces
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<T>(T domainEvent) where T : IDomainEvent;
    }
}