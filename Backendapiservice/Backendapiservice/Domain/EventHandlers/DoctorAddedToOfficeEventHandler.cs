using MediatR;
using Microsoft.Extensions.Logging;
using Backendapiservice.Domain.Events;

namespace Backendapiservice.Domain.EventHandlers
{
    public class DoctorAddedToOfficeEventHandler : INotificationHandler<DoctorAddedToOfficeEvent>
    {
        private readonly ILogger<DoctorAddedToOfficeEventHandler> _logger;

        public DoctorAddedToOfficeEventHandler(ILogger<DoctorAddedToOfficeEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DoctorAddedToOfficeEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Doctor {DoctorName} (ID: {DoctorId}) was added to office {OfficeName} (ID: {OfficeId})",
                notification.DoctorName,
                notification.DoctorId,
                notification.OfficeName,
                notification.OfficeId);

            return Task.CompletedTask;
        }
    }
}