using FluentValidation;
using ShopFront.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFront.Application.User.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Id)
                .Must(UserIdExistInDB)
                .WithMessage("User don't exist in database.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .Must(BeUniqueEmail)
                .WithMessage("This email was already registered.");

            RuleFor(x => x.FirstName)
                .Length(2, 45)
                .Matches(@"^[A-Za-z][A-Za-z\s-]*$")
                .WithMessage("First name must be between 2 and 45 characters long, start with a letter, and only contain Latin letters, spaces, and hyphens.");

            RuleFor(x => x.LastName)
                .Length(2, 45)
                .Matches(@"^[A-Za-z][A-Za-z\s-]*$")
                .WithMessage("Last name must be between 2 and 45 characters long, start with a letter, and only contain Latin letters, spaces, and hyphens.");

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

        private bool UserIdExistInDB(Guid id)
        {
            return _userRepository.GetUserById(id).Result is not null;
        }
    }
}
