using SBS.Domain.Entities;

namespace SBS.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
