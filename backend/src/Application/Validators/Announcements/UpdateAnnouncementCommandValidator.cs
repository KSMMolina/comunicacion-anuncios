using Communication.Announcements.Application.Features.Announcements.Commands;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Announcements;

public class UpdateAnnouncementCommandValidator : AbstractValidator<UpdateAnnouncementCommand>
{
    public UpdateAnnouncementCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Request).SetValidator(new AnnouncementUpdateRequestValidator());
    }
}
