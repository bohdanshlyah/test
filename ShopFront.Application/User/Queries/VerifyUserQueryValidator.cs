using FluentValidation;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Repositories;

public class VerifyUserQueryValidator : AbstractValidator<VerifyUserQuery>
{
    private readonly IUserRepository _userRepository;

    public VerifyUserQueryValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .Must(BeUniqueEmail)
            .WithMessage("Email does not exist.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(6, 255)
            .WithMessage("Password must be between 6 and 255 characters.");
    }

    private bool BeUniqueEmail(string email)
    {
        return _userRepository.UserExistsByEmail(email).Result;
    }
}
