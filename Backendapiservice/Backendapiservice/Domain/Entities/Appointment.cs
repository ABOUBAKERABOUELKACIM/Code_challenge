namespace Backendapiservice.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled
        public string Notes { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int? CreatedByAssistantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Assistant? CreatedByAssistant { get; set; }
    }
}
