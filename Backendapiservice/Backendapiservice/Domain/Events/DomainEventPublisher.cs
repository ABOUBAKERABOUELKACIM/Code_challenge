using MediatR;
using Backendapiservice.Domain.Events;
using Backendapiservice.Domain.Interfaces;

namespace Backendapiservice.Infrastructure.Events
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMediator _mediator;

        public DomainEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync<T>(T domainEvent) where T : IDomainEvent
        {
            await _mediator.Publish(domainEvent);
        }
    }
}