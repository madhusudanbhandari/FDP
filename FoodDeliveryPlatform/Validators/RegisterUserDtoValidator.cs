using FluentValidation;
using FDP.Dtos.User;

namespace FDP.Validators;


public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x=>x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required");

        RuleFor(x=>x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required");

        RuleFor(x=>x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(x=>x.Address)
            .NotEmpty()
            .WithMessage("Address cannot be empty");

        RuleFor(x=>x.Password)
            .NotEmpty()
            .WithMessage("Password Cannot be empty")
            .MinimumLength(8)
            .WithMessage("password must have more then 8 characters");

    }
}