namespace Communication.Announcements.Application.Common;

public class PagedResult<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    public int Total { get; init; }
    public int Page { get; init; }
    public int Size { get; init; }
}
