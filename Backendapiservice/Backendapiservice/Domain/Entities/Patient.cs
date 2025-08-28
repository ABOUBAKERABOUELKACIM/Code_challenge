﻿namespace Backendapiservice.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<Appointment> Appointments { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
    }
}
