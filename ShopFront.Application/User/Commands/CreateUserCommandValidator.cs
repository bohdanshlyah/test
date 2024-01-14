using FluentValidation;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Commands
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .Must(BeUniqueEmail)
                .WithMessage("This email was already registered.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 45)
                .Matches(@"^[A-Za-z][A-Za-z\s-]*$")
                .WithMessage("First name must be between 2 and 45 characters long, start with a letter, and only contain Latin letters, spaces, and hyphens.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 45)
                .Matches(@"^[A-Za-z][A-Za-z\s-]*$")
                .WithMessage("Last name must be between 2 and 45 characters long, start with a letter, and only contain Latin letters, spaces, and hyphens.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(6, 255)
                .Matches(@"^[A-Za-z0-9]+$")
                .WithMessage("Your password must contain Latin letters and digits")
                .Must((user, password) => !password.Contains(user.FirstName))
                .WithMessage("Your password must not contain your first name")
                .Must((user, password) => !password.Contains(user.LastName))
                .WithMessage("Your password must not contain your last name")
                .Must((user, password) => !password.Contains(user.Email))
                .WithMessage("Your password must not contain your email");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+\d{1,3}[\s-]?\d{9}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Phone must contain country code and 9 digits. Spaces and hyphens are allowed");

            RuleFor(x => x.TelegramUsername)
                .Must(username => username.StartsWith("@"))
                .WithMessage("Username must start with '@'.")
                .Length(5, 32)
                .WithMessage("Username must be between 5 and 32 characters long.")
                .Matches(@"^@[a-zA-Z]([a-zA-Z0-9_]*[a-zA-Z0-9])?$")
                .WithMessage("Username must start with '@' followed by a letter, and can only consist of letters, numbers, underscores (_), but cannot end with an underscore or have consecutive underscores.")
                .When(x => !string.IsNullOrEmpty(x.TelegramUsername));
        }

        private bool BeUniqueEmail(string email)
        {
            return !_userRepository.UserExistsByEmail(email).Result;
        }
    }
}
