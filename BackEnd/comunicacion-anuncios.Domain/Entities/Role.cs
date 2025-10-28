namespace comunicacion_anuncios.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}