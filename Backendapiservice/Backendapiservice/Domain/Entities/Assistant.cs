namespace Backendapiservice.Domain.Entities
{
    public class Assistant : User
    {
        public int DoctorId { get; set; }
        

        // Navigation properties
        public Doctor Doctor { get; set; } = null!;
        public List<Appointment> CreatedAppointments { get; set; } = new();

        public Assistant()
        {
            Role = "Assistant"; // Set default role
        }
    }
}