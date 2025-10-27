namespace comunicacion_anuncios.Domain.Entities
{
    public sealed class Attachment
    {
        public Guid Id { get; private set; }
        public Guid AnnouncementId { get; private set; }
        public string FileName { get; private set; } = default!;
        public string FileUrl { get; private set; } = default!;
        public DateTime UploadedAt { get; private set; } = DateTime.UtcNow;
    }
}
