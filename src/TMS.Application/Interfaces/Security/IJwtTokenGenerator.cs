using TMS.Domain.Entities;

namespace TMS.Application.Interfaces.Security;

/// <summary>
/// Abstraction over JWT creation. Implemented in Infrastructure so the
/// Application layer stays free of token/crypto libraries.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a signed JWT carrying the user's identity claims, together with
    /// the token's expiry instant.
    /// </summary>
    AuthTokenResult GenerateToken(User user);
}
