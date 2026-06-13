using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.DTOs.Auth;
using TMS.Application.Interfaces.Services;
using TMS.Shared.Models;

namespace TMS.API.Controllers;

/// <summary>Authentication endpoints. Anonymous by design.</summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Validates credentials and returns a JWT on success.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);

        if (result is null)
            return Unauthorized(ApiResponse.Failure<LoginResponseDto>("Invalid username or password."));

        return Ok(ApiResponse.Success(result, "Login successful."));
    }
}
