using Xunit;
using Moq;
using FluentValidation.TestHelper;
using ShopFront.Application.User.Commands;
using ShopFront.Domain.Repositories;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.UserExistsByEmail(It.IsAny<string>())).ReturnsAsync(false);
        _validator = new CreateUserCommandValidator(mockRepo.Object);
    }

    #region Email tests
    [Fact]
    public void Email_WhenEmpty_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Password = "TestPassword123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Email_WhenInvalidFormat_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "invalidemail",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public void Email_WhenAlreadyRegistered_ReturnsValidationError()
    {
        // Arrange
        var email = "alreadyregistered@example.com";
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.UserExistsByEmail(email)).ReturnsAsync(true);
        var validator = new CreateUserCommandValidator(mockRepo.Object);

        var command = new CreateUserCommand
        {
            Email = email,
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("This email was already registered.");
    }
    #endregion

    #region FirstName tests
    [Fact]
    public void FirstName_WhenEmpty_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    public void FirstName_InvalidCharacters_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "123",
            LastName = "Doe", 
            Email = "test@example.com", 
            Password = "Password123" 
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FirstName).WithErrorMessage("First name must be between 2 and 45 characters long, start with a letter, and only contain Latin letters, spaces, and hyphens.");
    }
    #endregion

    #region LastName tests
    [Fact]
    public void LastName_WhenEmpty_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "",
            Email = "test@example.com",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    public void LastName_InvalidCharacters_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "1234",
            Email = "test@example.com",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LastName).WithErrorMessage("Last name must be between 2 and 45 characters long, start with a letter, and only contain Latin letters, spaces, and hyphens.");
    }
    #endregion

    #region Password tests
    [Fact]
    public void Password_WhenEmpty_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = ""
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Password_TooShort_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Pas1"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password)
              .WithErrorMessage("'Password' must be between 6 and 255 characters. You entered 4 characters.");
    }


    [Fact]
    public void Password_ContainsFirstName_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "John1234"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage("Your password must not contain your first name");
    }
    #endregion

    #region PhoneNumber tests
    [Fact]
    public void PhoneNumber_InvalidFormat_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Password123",
            PhoneNumber = "123456"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PhoneNumber).WithErrorMessage("Phone must contain country code and 9 digits. Spaces and hyphens are allowed");
    }

    [Fact]
    public void PhoneNumber_WhenEmpty_NoValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Password123",
            PhoneNumber = ""
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.PhoneNumber);
    }
    #endregion

    #region TelegramUsername tests
    [Fact]
    public void TelegramUsername_InvalidFormat_ReturnsValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Password123",
            TelegramUsername = "invalidUsername"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.TelegramUsername).WithErrorMessage("Username must start with '@'.");
    }

    [Fact]
    public void TelegramUsername_WhenEmpty_NoValidationError()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Password = "Password123",
            TelegramUsername = ""
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.TelegramUsername);
    }
    #endregion
}
