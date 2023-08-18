using BlazorSozluk.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Applicaition.Features.Commands.UserCommands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.EmailAddress)
             .NotNull()
             .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
             .WithMessage("{PropertyName} not a valid email address");

        RuleFor(command => command.Password)
            .NotNull()
            .MinimumLength(8)
            .WithMessage("{PropertyName} should at least be {MinLenght} characters");
    }
}
