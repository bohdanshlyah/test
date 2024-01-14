using FluentValidation.TestHelper;
using Moq;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Repositories;

public class VerifyUserQueryValidatorTests
{
    private readonly VerifyUserQueryValidator _validator;

    public VerifyUserQueryValidatorTests()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.UserExistsByEmail(It.IsAny<string>())).ReturnsAsync(true);
        _validator = new VerifyUserQueryValidator(mockRepo.Object);
    }

    #region Email tests
    [Fact]
    public void Email_WhenEmpty_ReturnsValidationError()
    {
        // Arrange
        var query = new VerifyUserQuery
        {
            Email = "",
            Password = "TestPassword123"
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Email).WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Email_WhenInvalidFormat_ReturnsValidationError()
    {
        // Arrange
        var query = new VerifyUserQuery
        {
            Email = "invalidemail",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Email).WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public void Email_WhenDoesNotExist_ReturnsValidationError()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.UserExistsByEmail(email)).ReturnsAsync(false);
        var validator = new VerifyUserQueryValidator(mockRepo.Object);

        var query = new VerifyUserQuery
        {
            Email = email,
            Password = "Password123"
        };

        // Act
        var result = validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Email).WithErrorMessage("Email does not exist.");
    }
    #endregion


    #region Password tests
    [Fact]
    public void Password_WhenEmpty_ReturnsValidationError()
    {
        // Arrange
        var query = new VerifyUserQuery
        {
            Email = "test@example.com",
            Password = ""
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Password).WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Password_TooShort_ReturnsValidationError()
    {
        // Arrange
        var query = new VerifyUserQuery
        {
            Email = "test@example.com",
            Password = "Pas12"
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Password).WithErrorMessage("Password must be between 6 and 255 characters.");
    }

    [Fact]
    public void Password_Valid_DoesNotReturnValidationError()
    {
        // Arrange
        var query = new VerifyUserQuery
        {
            Email = "test@example.com",
            Password = "ValidPassword123"
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Password);
    }
    #endregion
}
