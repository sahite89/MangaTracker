using Xunit;
using FluentAssertions;
using MangaTracker.Application.Features.Users.RegisterUser;

namespace MangaTracker.Test.Application.Commands.RegisterUser
{
    public class RegisterUserCommandValidatorTests
    {
        private readonly RegisterUserCommandValidator _validator;

        public RegisterUserCommandValidatorTests()
        {
            _validator = new RegisterUserCommandValidator();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("Test")]
        [InlineData("TEST")]
        [InlineData("usuario123")]
        public void Should_Not_Have_Error_When_UserName_Is_Valid(string userName)
        {
            var command = new RegisterUserCommand
            {
                UserName = userName,
                Email = "test@test.com",
                Password = "Password123!"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenUsernameIsEmpty()
        {
            // Arrange
            var command = new RegisterUserCommand
            {               
                UserName = "",
                Email =  "test@example.com",
                Password = "ValidPassword123!"
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserName");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "UserName is required.");

        }

        [Fact]
        public void Validate_ShouldReturnError_WhenUsernameIsTooShort()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "ab",
                Email = "test@example.com",
                Password = "ValidPassword123!"
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserName");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "UserName must be at least 3 characters.");
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenUsernameIsTooLarge()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "a".PadRight(51, 'b'), // 51 characters
                Email = "test@example.com",
                Password = "ValidPassword123!"
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserName");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "UserName must not exceed 50 characters.");
        }

        [Theory]
        [InlineData("test@test.com")]
        [InlineData("usuario@gmail.com")]
        [InlineData("usuario.test@test.com")]
        [InlineData("test1245@test.es")]
        public void Should_Not_Have_Error_When_Email_Is_Valid(string email)
        {
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = email,
                Password = "Password123!"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenEmailIsEmpty()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "",
                Password = "ValidPassword123!"
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Email is required.");
        }

        [Theory]
        [InlineData("test.com")]
        [InlineData("usuario@")]
        [InlineData("test")]
        [InlineData("@hotmail.com")]
        public void Validate_ShouldReturnError_WhenEmailNotValidFormat(string email)
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = email,
                Password = "ValidPassword123!"
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordIsEmpty()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@example.com",
                Password = ""
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Password is required.");
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordIsTooShort()
        {
            // Arrange
            var command = new RegisterUserCommand
            {   
                UserName = "test",
                Email = "test@example.com",
                Password = "VdPa12!"
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Password must be at least 8 characters.");
        }

        [Fact]
        public void Validate_ShouldReturnError_WhenPasswordIsTooLarge()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@example.com",
                Password = "VeryLongPassword123!".PadRight(101, 'a')
            };
            // Act
            var result = _validator.Validate(command);
            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password");
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Password must not exceed 100 characters.");
        }

        [Fact]
        public void Should_Pass_Validation_When_Command_Is_Valid()
        {
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@test.com",
                Password = "Password123."
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }
    }


}
