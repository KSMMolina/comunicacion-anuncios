using Communication.Announcements.Application.Features.Announcements.Queries;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class GetReadersByAnnouncementQueryValidator : AbstractValidator<GetReadersByAnnouncementQuery>
{
    public GetReadersByAnnouncementQueryValidator()
    {
        RuleFor(x => x.AnnouncementId).NotEmpty();
    }
}
