using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public abstract class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    protected RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Incorrect refresh token");
    }
}