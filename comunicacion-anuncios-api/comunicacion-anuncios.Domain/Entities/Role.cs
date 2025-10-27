namespace comunicacion_anuncios.Domain.Entities
{
    public sealed class Role
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public ICollection<User> Users { get; } = new List<User>();
    }
}
