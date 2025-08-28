namespace Backendapiservice.Domain.Entities
{
    public class Doctor : User
    {
        public string Specialization { get; set; } = string.Empty;
        public int OfficeId { get; set; }
        

        // Navigation properties
        public Office Office { get; set; } = null!;
        public List<Assistant> Assistants { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();

        public Doctor()
        {
            Role = "Doctor"; // Set default role
        }
    }
}