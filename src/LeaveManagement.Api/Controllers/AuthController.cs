using LeaveManagement.Api.DTOs;
using LeaveManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class AuthController(IConfiguration configuration, ITokenService tokens) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var valid = request.Username == configuration["Admin:Username"] && request.Password == configuration["Admin:Password"];
        return valid ? Ok(new { token = tokens.Create(request.Username) }) : Unauthorized(new { message = "Invalid credentials" });
    }
}
