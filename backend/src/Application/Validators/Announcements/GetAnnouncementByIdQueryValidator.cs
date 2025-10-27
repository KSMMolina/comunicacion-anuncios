using Communication.Announcements.Application.Features.Announcements.Queries;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class GetAnnouncementByIdQueryValidator : AbstractValidator<GetAnnouncementByIdQuery>
{
    public GetAnnouncementByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
