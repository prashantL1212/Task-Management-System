namespace TMS.Application.DTOs.Auth;

/// <summary>
/// Credentials submitted to <c>POST /api/auth/login</c>.
/// Validated by <c>LoginRequestValidator</c>.
/// </summary>
public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
