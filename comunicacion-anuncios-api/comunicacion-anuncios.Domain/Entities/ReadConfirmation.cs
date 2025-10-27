namespace comunicacion_anuncios.Domain.Entities
{
    public sealed class ReadConfirmation
    {
        public Guid Id { get; private set; }
        public Guid AnnouncementId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ReadAt { get; private set; } = DateTime.UtcNow;
    }
}
