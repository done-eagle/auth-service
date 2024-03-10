using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public class GetAccessTokenDtoValidator : AbstractValidator<GetAccessTokenRequestDto>
{
    public GetAccessTokenDtoValidator()
    {
        RuleFor(x => x.AuthCode)
            .NotEmpty().WithMessage("Incorrect authorization_code");
        RuleFor(x => x.CodeVerifier)
            .NotEmpty().WithMessage("Incorrect code_verifier");
    }
}