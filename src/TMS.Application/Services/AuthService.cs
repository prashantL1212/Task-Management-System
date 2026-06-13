using TMS.Application.DTOs.Auth;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Interfaces.Security;
using TMS.Application.Interfaces.Services;

namespace TMS.Application.Services;

/// <summary>
/// Authentication business logic. Validates credentials and issues a JWT using
/// only Application abstractions — no configuration, JWT, or crypto libraries.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
            return null; // unknown username — caller maps to 401

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return null; // wrong password — caller maps to 401

        var tokenResult = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = tokenResult.Token,
            // Expiry comes straight from the token generator's result —
            // no config read, no JWT decode, no hardcoded value.
            ExpiresAt = tokenResult.ExpiresAtUtc,
            Username = user.Username,
            Role = user.Role
        };
    }
}
