using FluentValidation;
using TMS.Application.DTOs.Auth;

namespace TMS.Application.Validators;

/// <summary>Validation rules for login credentials.</summary>
public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
