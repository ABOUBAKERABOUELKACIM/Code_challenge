using MediatR;
using Microsoft.Extensions.Logging;
using Backendapiservice.Domain.Events;

namespace Backendapiservice.Domain.EventHandlers
{
    public class AppointmentCreatedEventHandler : INotificationHandler<AppointmentCreatedEvent>
    {
        private readonly ILogger<AppointmentCreatedEventHandler> _logger;

        public AppointmentCreatedEventHandler(ILogger<AppointmentCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(AppointmentCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "New appointment created: {AppointmentId} for Doctor {DoctorId} and Patient {PatientId} on {DateTime}",
                notification.AppointmentId,
                notification.DoctorId,
                notification.PatientId,
                notification.AppointmentDateTime);

            return Task.CompletedTask;
        }
    }
}