namespace Backendapiservice.Application.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Specialization { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OfficeId { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AssistantsCount { get; set; }
        public int AppointmentsCount { get; set; }
    }

    public class CreateDoctorDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public int OfficeId { get; set; }
    }
}