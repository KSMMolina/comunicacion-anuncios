namespace comunicacion_anuncios.Domain.Entities
{
    public sealed class Announcement
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = default!;
        public string Message { get; private set; } = default!;
        public string? TargetGroup { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public bool IsActive { get; private set; } = true;
        public ICollection<Attachment> Attachments { get; } = new List<Attachment>();
    }
}
