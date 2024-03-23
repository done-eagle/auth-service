using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public class ChangeUserPasswordDtoValidator : AbstractValidator<ChangeUserPasswordRequestDto>
{
    public ChangeUserPasswordDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Incorrect userId");
        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(8).WithMessage("Incorrect password");
    }
}