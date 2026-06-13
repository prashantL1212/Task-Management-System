using TMS.Domain.Common;

namespace TMS.Domain.Entities;

/// <summary>
/// Application user. Used exclusively for authentication and authorization.
/// Has no relationship to <see cref="TaskItem"/> (task assignment is free-text).
/// </summary>
public class User : BaseEntity
{
    /// <summary>Unique login name. Required.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Hashed password (never stored in plain text). Required.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Authorization role, e.g. "Admin". Defaults to "User".</summary>
    public string Role { get; set; } = "User";
}
