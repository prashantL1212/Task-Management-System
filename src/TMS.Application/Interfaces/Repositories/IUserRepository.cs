using TMS.Domain.Entities;

namespace TMS.Application.Interfaces.Repositories;

/// <summary>
/// Persistence contract for users. Used by authentication only — there is no
/// relationship between users and tasks.
/// </summary>
public interface IUserRepository
{
    /// <summary>Returns the user with the given username, or null if none.</summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
