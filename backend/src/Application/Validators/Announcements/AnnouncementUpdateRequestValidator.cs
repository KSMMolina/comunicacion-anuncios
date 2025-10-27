using Communication.Announcements.Application.DTOs.Announcements;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class AnnouncementUpdateRequestValidator : AbstractValidator<AnnouncementUpdateRequest>
{
    public AnnouncementUpdateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.TargetGroup)
            .MaximumLength(100);
    }
}
