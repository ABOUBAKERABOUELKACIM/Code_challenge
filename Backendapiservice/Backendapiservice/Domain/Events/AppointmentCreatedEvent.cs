namespace Backendapiservice.Domain.Events
{
    public class AppointmentCreatedEvent : IDomainEvent
    {
        public int AppointmentId { get; }
        public int DoctorId { get; }
        public int PatientId { get; }
        public DateTime AppointmentDateTime { get; }
        public DateTime OccurredOn { get; }

        public AppointmentCreatedEvent(int appointmentId, int doctorId, int patientId, DateTime appointmentDateTime)
        {
            AppointmentId = appointmentId;
            DoctorId = doctorId;
            PatientId = patientId;
            AppointmentDateTime = appointmentDateTime;
            OccurredOn = DateTime.UtcNow;
        }
    }
}