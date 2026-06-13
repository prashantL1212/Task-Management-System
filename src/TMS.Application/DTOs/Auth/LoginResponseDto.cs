namespace TMS.Application.DTOs.Auth;

/// <summary>
/// Result of a successful login: the JWT and basic identity details the SPA
/// needs (the token itself is stored in localStorage by the client).
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
