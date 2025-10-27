namespace comunicacion_anuncios.Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public Guid RoleId { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Role Role { get; private set; } = default!;
    }
}
