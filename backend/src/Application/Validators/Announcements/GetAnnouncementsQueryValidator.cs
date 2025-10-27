using Communication.Announcements.Application.Features.Announcements.Queries;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class GetAnnouncementsQueryValidator : AbstractValidator<GetAnnouncementsQuery>
{
    public GetAnnouncementsQueryValidator()
    {
        RuleFor(x => x.Filter.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Filter.Size).InclusiveBetween(1, 100);
    }
}
