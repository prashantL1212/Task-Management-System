namespace TMS.Application.Interfaces.Security;

/// <summary>
/// Abstraction over password hashing/verification. Implemented in Infrastructure
/// (BCrypt) so the Application layer is independent of the hashing library.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Hashes a plain-text password for storage.</summary>
    string Hash(string password);

    /// <summary>Verifies a plain-text password against a stored hash.</summary>
    bool Verify(string password, string passwordHash);
}
