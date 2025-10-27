using Communication.Announcements.Application.DTOs.Auth;
using Communication.Announcements.Application.Features.Auth.Commands;
using FluentValidation;

namespace Communication.Announcements.Application.Validators.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new AuthLoginRequestValidator());
    }
}
