using Backendapiservice.Domain.Entities;

namespace Backendapiservice.Infrastructure.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}