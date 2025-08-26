using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Robotic.Folklift.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Robotic.Folklift.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _cfg;
        public JwtTokenService(IConfiguration cfg) => _cfg = cfg;

        public (string token, DateTime expiresAt) CreateToken(int userId, string username)
        {
            var secret = _cfg["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not configured");
            var issuer = _cfg["Jwt:Issuer"] ?? "FolkliftApi";
            var audience = _cfg["Jwt:Audience"] ?? "FolkliftClient";
            var expires = DateTime.Now.AddHours(1);


            var claims = new List<Claim>
                                {
                                new(ClaimTypes.NameIdentifier, userId.ToString()),
                                new(ClaimTypes.Name, username)
                                };

            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, expires);
        }
    }
}
