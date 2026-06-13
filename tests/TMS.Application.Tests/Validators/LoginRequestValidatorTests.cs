using FluentAssertions;
using TMS.Application.DTOs.Auth;
using TMS.Application.Validators;

namespace TMS.Application.Tests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    public void Validate_ValidCredentials_Passes()
    {
        var result = _validator.Validate(new LoginRequestDto { Username = "admin", Password = "secret" });
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyUsername_Fails()
    {
        var result = _validator.Validate(new LoginRequestDto { Username = "", Password = "secret" });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LoginRequestDto.Username));
    }

    [Fact]
    public void Validate_EmptyPassword_Fails()
    {
        var result = _validator.Validate(new LoginRequestDto { Username = "admin", Password = "" });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LoginRequestDto.Password));
    }

    [Fact]
    public void Validate_BothFieldsEmpty_FailsForBoth()
    {
        var result = _validator.Validate(new LoginRequestDto { Username = "", Password = "" });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LoginRequestDto.Username));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LoginRequestDto.Password));
    }
}
