using Communication.Announcements.Application.Features.Announcements.Commands;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
{
    public CreateAnnouncementCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new AnnouncementCreateRequestValidator());
    }
}
