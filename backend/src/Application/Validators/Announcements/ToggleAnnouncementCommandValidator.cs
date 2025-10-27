using Communication.Announcements.Application.Features.Announcements.Commands;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class ToggleAnnouncementCommandValidator : AbstractValidator<ToggleAnnouncementCommand>
{
    public ToggleAnnouncementCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
