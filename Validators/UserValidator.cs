using System;
using System.Text.RegularExpressions;
using FluentValidation;
using platychat_dotnet.DTOs;

namespace platychat_dotnet.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName cannot be empty")
            .Must(fn => fn == fn.Trim()).WithMessage("FirstName cannot have leading or trailing spaces.");

        RuleFor(x => x.LastName)
            .Must(fn => fn == fn?.Trim()).WithMessage("LastName cannot have leading or trailing spaces.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Email must be of example@test.com type.")
            .Must(fn => fn == fn.Trim()).WithMessage("Email cannot have leading or trailing spaces.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Password can only contain alphabets and numbers");
    }
}

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Email must be of example@test.com type.")
            .Must(fn => fn == fn.Trim()).WithMessage("Email cannot have leading or trailing spaces.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Password can only contain alphabets and numbers");
    }
}
