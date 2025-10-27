using Communication.Announcements.Application.DTOs.Auth;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Auth;

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
