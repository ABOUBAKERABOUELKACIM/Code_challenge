namespace Backendapiservice.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int? CreatedByAssistantId { get; set; }
        public string? CreatedByAssistantName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateAppointmentDto
    {
        public DateTime AppointmentDateTime { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int? CreatedByAssistantId { get; set; }
    }

    public class UpdateAppointmentStatusDto
    {
        public string Status { get; set; } = string.Empty; // Scheduled, Completed, Cancelled
        public string? Notes { get; set; }
    }
}