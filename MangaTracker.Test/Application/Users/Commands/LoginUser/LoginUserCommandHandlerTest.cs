using FluentAssertions;
using MangaTracker.Application.Contracts.Infrastructure;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
using MangaTracker.Application.Features.Users.LoginUser;
using MangaTracker.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MangaTracker.Test.Application.Users.Commands.LoginUser
{
    public class LoginUserCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly LoginUserCommandHandler _loginUserCommandHandler;

        public LoginUserCommandHandlerTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _loginUserCommandHandler = new LoginUserCommandHandler(_passwordHasherMock.Object, _jwtProviderMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Return_Token_When_Credentials_Are_Valid()
        {

            var logginUserCommand = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "Password123."
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(logginUserCommand.Email))
                .ReturnsAsync(new User { Email = logginUserCommand.Email, PasswordHash = "hashed_password" });

            _passwordHasherMock
                .Setup(hasher => hasher.Verify(logginUserCommand.Password, "hashed_password"))
                .Returns(true);

            _jwtProviderMock
                .Setup(jwt => jwt.GenerateToken(It.IsAny<User>()))
                .Returns("generated_token");

            var userResponse = await _loginUserCommandHandler.HandleAsync(logginUserCommand, CancellationToken.None);

            _jwtProviderMock.Verify(
                    x => x.GenerateToken(It.IsAny<User>()),
                    Times.Once);

            userResponse.Success.Should().BeTrue();
            userResponse.loginUserDto!.Token.Should().Be("generated_token");

        }

        [Fact]
        public async Task Should_Return_Error_When_Credentials_Are_Invalid()
        {
            var logginUserCommand = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "WrongPassword123."
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(logginUserCommand.Email))
                .ReturnsAsync(new User { Email = logginUserCommand.Email, PasswordHash = "hashed_password" });

            _passwordHasherMock
                .Setup(hasher => hasher.Verify(logginUserCommand.Password, "hashed_password"))
                .Returns(false);

            var userResponse = await _loginUserCommandHandler.HandleAsync(logginUserCommand, CancellationToken.None);

            _jwtProviderMock.Verify(
                    x => x.GenerateToken(It.IsAny<User>()),
                    Times.Never);

            userResponse.Success.Should().BeFalse();
            userResponse.Message.Should().Be("Invalid email or password");
            userResponse.loginUserDto.Should().BeNull();
            Assert.Equal(ErrorCode.InvalidCredentials, userResponse.ErrorCode);

        }

        [Fact]
        public async Task Should_Return_Error_When_User_Does_Not_Exist()
        {
            var logginUserCommand = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "Password123."
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(logginUserCommand.Email))
                .ReturnsAsync((User)null);

            var userResponse = await _loginUserCommandHandler.HandleAsync(logginUserCommand, CancellationToken.None);

            _jwtProviderMock.Verify(
                    x => x.GenerateToken(It.IsAny<User>()),
                    Times.Never);

            userResponse.Success.Should().BeFalse();
            userResponse.Message.Should().Be("Invalid email or password");
            userResponse.loginUserDto.Should().BeNull();
            Assert.Equal(ErrorCode.InvalidCredentials, userResponse.ErrorCode);


        }

        [Fact]
        public async Task Should_Return_Error_When_User_Put_Empty_Email_And_Password()
        {
            var logginUserCommand = new LoginUserCommand
            {
                Email = "",
                Password = ""
            };

            var userResponse = await _loginUserCommandHandler.HandleAsync(logginUserCommand, CancellationToken.None);

            _jwtProviderMock.Verify(
                    x => x.GenerateToken(It.IsAny<User>()),
                    Times.Never);

            userResponse.Success.Should().BeFalse();
            userResponse.Message.Should().Be("Validation errors occurred");
            userResponse.loginUserDto.Should().BeNull();
            Assert.Equal(ErrorCode.ValidationError, userResponse.ErrorCode);

        }
    }
}
