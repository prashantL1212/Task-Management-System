using TMS.Application.Interfaces.Security;
using BCryptNet = BCrypt.Net.BCrypt;

namespace TMS.Infrastructure.Security;

/// <summary>BCrypt-based implementation of <see cref="IPasswordHasher"/>.</summary>
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCryptNet.HashPassword(password);

    public bool Verify(string password, string passwordHash) => BCryptNet.Verify(password, passwordHash);
}
