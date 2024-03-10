using AuthService.Api.Dto.Request;
using FluentValidation;

namespace AuthService.Api.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Incorrect username");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Incorrect firstname");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Incorrect lastname");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Incorrect phone number");
        
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("Incorrect email");
        
        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(8).WithMessage("Incorrect password");
    }
}