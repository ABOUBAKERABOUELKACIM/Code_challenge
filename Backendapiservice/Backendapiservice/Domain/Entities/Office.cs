using System.Numerics;

namespace Backendapiservice.Domain.Entities
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<Doctor> Doctors { get; set; } = new();
    }
}
