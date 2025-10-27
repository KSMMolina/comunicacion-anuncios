namespace comunicacion_anuncios.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Guid RoleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Role? Role { get; set; }
    public ICollection<Announcement> AnnouncementsCreated { get; set; } = new List<Announcement>();
    public ICollection<ReadConfirmation> ReadConfirmations { get; set; } = new List<ReadConfirmation>();
}