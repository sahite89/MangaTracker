using FluentAssertions;
using MangaTracker.Application.Contracts.Infrastructure;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.Users.RegisterUser;
using MangaTracker.Domain.Entities;
using Moq;
using Xunit;

namespace MangaTracker.Test.Application.Commands.Users.RegisterUser
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterUserCommandHandler _registerUserHandler;


        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _registerUserHandler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Should_Create_user_When_Request_Is_Valid()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@test.com",
                Password = "Password123."
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            _passwordHasherMock
                .Setup(x => x.Hash(command.Password))
                .Returns("HashedPassword123.");

            // Act
            await _registerUserHandler.HandleAsync(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once());

        }

        [Fact]
        public async Task Should_Return_Error_When_User_Already_Exists()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@test.com",
                Password = "Password123."
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "existingUser",
                Email = "test@test.com",
                PasswordHash = "HashedPassword123."
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(command.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _registerUserHandler.HandleAsync(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User already exists");

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public async Task Should_Hash_Password_When_Creating_User()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                UserName = "test",
                Email = "test@test.com",
                Password = "Password123."
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
               .Setup(x => x.AddAsync(It.IsAny<User>()))
               .ReturnsAsync((User user) => user);

            _passwordHasherMock
                .Setup(x => x.Hash(command.Password))
                .Returns("HashedPassword123.");

            // Act
            await _registerUserHandler.HandleAsync(command, CancellationToken.None);

            // Assert
            _passwordHasherMock.Verify(
                x => x.Hash(command.Password),
                Times.Once());

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.Is<User>(
                    u => u.PasswordHash == "HashedPassword123.")),
                Times.Once);
        }
    }
}