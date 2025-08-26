using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Application.Dtos;
using Robotic.Folklift.Application.Interfaces;
using LoginRequest = Robotic.Folklift.Application.Dtos.LoginRequest;


namespace Robotic.Folklift.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAppDbContext _db; private readonly IJwtTokenService _jwt;
    public AuthController(IAppDbContext db, IJwtTokenService jwt) { _db = db; _jwt = jwt; }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users.SingleOrDefaultAsync(x => x.Username == req.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash)) return Unauthorized();
        var (token, exp) = _jwt.CreateToken(user.Id, user.Username);
        return Ok(new LoginResponse(token, exp));
    }
}