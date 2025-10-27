using Communication.Announcements.Application.DTOs.Auth;
using Communication.Announcements.Application.Validators.Auth;
using FluentAssertions;
using Xunit;

namespace Communication.Announcements.Application.Tests.Validators;

public class AuthLoginRequestValidatorTests
{
    private readonly AuthLoginRequestValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Email_Invalid()
    {
        var model = new AuthLoginRequest { Email = "bad-email", Password = "123" };
        var result = _validator.Validate(model);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Request_Is_Valid()
    {
        var model = new AuthLoginRequest { Email = "user@example.com", Password = "123" };
        var result = _validator.Validate(model);
        result.IsValid.Should().BeTrue();
    }
}
