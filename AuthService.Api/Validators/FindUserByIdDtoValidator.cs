using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public abstract class FindUserByIdDtoValidator : AbstractValidator<FindUserByIdRequestDto>
{
    protected FindUserByIdDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Incorrect userId");
    }
}