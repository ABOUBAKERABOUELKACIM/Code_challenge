namespace Backendapiservice.Domain.Events
{
    public class DoctorAddedToOfficeEvent : IDomainEvent
    {
        public int DoctorId { get; }
        public int OfficeId { get; }
        public string DoctorName { get; }
        public string OfficeName { get; }
        public DateTime OccurredOn { get; }

        public DoctorAddedToOfficeEvent(int doctorId, int officeId, string doctorName, string officeName)
        {
            DoctorId = doctorId;
            OfficeId = officeId;
            DoctorName = doctorName;
            OfficeName = officeName;
            OccurredOn = DateTime.UtcNow;
        }
    }
}