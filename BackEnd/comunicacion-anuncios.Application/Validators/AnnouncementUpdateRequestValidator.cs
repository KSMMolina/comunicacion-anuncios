using FluentValidation;
using comunicacion_anuncios.Application.DTOs;

namespace comunicacion_anuncios.Application.Validators;

public class AnnouncementUpdateRequestValidator : AbstractValidator<AnnouncementUpdateRequest>
{
    public AnnouncementUpdateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(200);

        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(5000);
    }
}