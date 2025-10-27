using Communication.Announcements.Application.Features.Announcements.Commands;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class ConfirmReadCommandValidator : AbstractValidator<ConfirmReadCommand>
{
    public ConfirmReadCommandValidator()
    {
        RuleFor(x => x.AnnouncementId).NotEmpty();
    }
}
