namespace Communication.Announcements.Domain.Entities;

/// <summary>
/// Represents a role allowed in the announcements module.
/// </summary>
public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<User> Users { get; set; } = new HashSet<User>();
}
