namespace Robotic.Folklift.Application.Interfaces
{
    public interface IJwtTokenService
    {
        (string token, DateTime expiresAt) CreateToken(int userId, string username);
    }
}
