namespace TMS.Application.Interfaces.Security;

/// <summary>
/// Result of generating an authentication token: the signed JWT and the exact
/// UTC instant it expires. Returning the expiry alongside the token lets the
/// auth flow report it without re-reading configuration or decoding the JWT —
/// the token generator (which owns the lifetime) is the single source of truth.
/// </summary>
public record AuthTokenResult(string Token, DateTime ExpiresAtUtc);
