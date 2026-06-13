namespace TMS.Domain.Common;

/// <summary>
/// Base type for persistable domain entities. Holds the identity and creation
/// timestamp shared by every entity so they are not repeated on each one.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Primary key (database identity).</summary>
    public int Id { get; set; }

    /// <summary>UTC timestamp set when the entity is first created.</summary>
    public DateTime CreatedDate { get; set; }
}
