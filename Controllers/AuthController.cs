using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Controllers;

[ApiController]
[Route("api/users")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtService _jwtService;

    public AuthController(AuthService authService, JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        var user = _authService.ValidateUser(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new LoginResponseDto { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        return Ok(new
        {
            Id = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
            Email = User.FindFirstValue(JwtRegisteredClaimNames.Email),
            Username = User.FindFirstValue("username")
        });
    }
}
