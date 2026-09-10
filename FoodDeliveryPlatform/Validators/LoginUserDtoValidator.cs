using FluentValidation;
using FDP.Dtos.User;
using System.Data;

public class LoginDtoValidator: AbstractValidator<LoginUserDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x=>x.Email)
            .NotEmpty()
            .WithMessage("Email Cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x=>x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}