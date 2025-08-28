namespace Backendapiservice.Domain.Entities
{
    public class Admin : User
    {
        
        public bool IsActive { get; set; } = true;


        public Admin()
        {
            Role = "Admin"; // Set default role in constructor
        }
    }
}