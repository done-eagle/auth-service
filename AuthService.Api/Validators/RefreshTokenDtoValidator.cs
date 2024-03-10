using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Incorrect refresh token");
    }
}