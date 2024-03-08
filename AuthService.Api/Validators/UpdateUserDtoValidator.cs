using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public abstract class UpdateUserDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    protected UpdateUserDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Incorrect userId");
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("Incorrect email");
        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(8).WithMessage("Incorrect password");
    }
}