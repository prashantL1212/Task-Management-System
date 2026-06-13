using TMS.Application.DTOs.Auth;

namespace TMS.Application.Interfaces.Services;

/// <summary>
/// Authentication operations. Implementation lives in the Application layer and
/// depends on <c>IUserRepository</c>, <c>IPasswordHasher</c> and
/// <c>IJwtTokenGenerator</c>.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Validates credentials and issues a JWT. Returns null when the username
    /// is unknown or the password does not match.
    /// </summary>
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}
