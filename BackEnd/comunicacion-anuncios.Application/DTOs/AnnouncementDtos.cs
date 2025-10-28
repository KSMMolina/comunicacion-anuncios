namespace comunicacion_anuncios.Application.DTOs;

public record AnnouncementCreateRequest(string Title, string Message, string? TargetGroup);
public record AnnouncementUpdateRequest(string Title, string Message, string? TargetGroup);
public record AnnouncementResponse(
    Guid Id,
    string Title,
    string Message,
    string? TargetGroup,
    bool IsActive,
    DateTime CreatedAt,
    Guid CreatedBy,
    string CreatedByName);
public record ConfirmReadResponse(Guid AnnouncementId, Guid UserId, DateTime ReadAt);
public record ReaderItem(Guid UserId, string FullName, string Email, bool Read, DateTime? ReadAt);