namespace Backendapiservice.Domain.Events
{
    public class AppointmentStatusChangedEvent : IDomainEvent
    {
        public int AppointmentId { get; }
        public string OldStatus { get; }
        public string NewStatus { get; }
        public int DoctorId { get; }
        public int PatientId { get; }
        public DateTime OccurredOn { get; }

        public AppointmentStatusChangedEvent(int appointmentId, string oldStatus, string newStatus, int doctorId, int patientId)
        {
            AppointmentId = appointmentId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            DoctorId = doctorId;
            PatientId = patientId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}