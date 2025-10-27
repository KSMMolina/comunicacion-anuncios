namespace Communication.Announcements.Application.Common;

public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Search { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? ActiveOnly { get; set; }
}
