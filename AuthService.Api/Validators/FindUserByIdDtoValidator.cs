using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public class FindUserByIdDtoValidator : AbstractValidator<FindUserByIdRequestDto>
{
    public FindUserByIdDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Incorrect userId");
    }
}