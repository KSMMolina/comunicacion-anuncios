using FluentValidation;
using comunicacion_anuncios.Application.DTOs;

namespace comunicacion_anuncios.Application.Validators;

public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
{
    public AuthLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}