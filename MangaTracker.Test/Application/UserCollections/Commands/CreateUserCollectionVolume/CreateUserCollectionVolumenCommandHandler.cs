using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume;
using MangaTracker.Domain.Entities;
using Moq;
using Xunit;

namespace MangaTracker.Test.Application.UserCollections.Commands.CreateUserCollectionVolume
{
    public class CreateUserCollectionVolumenCommandHandler
    {
        private readonly Mock<IUserCollectionVolumeRepository> _userCollectionVolumeRepositoryMock;
        private readonly CreateUserCollectionVolumeCommandHandler _createUserCollectionVolumeHandler;
        private readonly Mock<IUserCollectionRepository> _userCollectionRepositoryMock;

        public CreateUserCollectionVolumenCommandHandler()
        {
            _userCollectionVolumeRepositoryMock = new Mock<IUserCollectionVolumeRepository>();
            _userCollectionRepositoryMock = new Mock<IUserCollectionRepository>();
            _createUserCollectionVolumeHandler = new CreateUserCollectionVolumeCommandHandler(_userCollectionVolumeRepositoryMock.Object, _userCollectionRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Create_Volume_When_Request_Is_Valid() 
        {
            var command = new CreateUserCollectionVolumeCommand
            {
                UserId = Guid.NewGuid(),
                MangaId = Guid.NewGuid(),
                VolumeNumber = 1
            };

            UserCollectionVolume? savedVolumen = null;

            _userCollectionVolumeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UserCollectionVolume>()))
                .Callback<UserCollectionVolume>(volume => savedVolumen = volume)
                .ReturnsAsync((UserCollectionVolume volume) => volume);

            var result = await _createUserCollectionVolumeHandler.HandleAsync(command, CancellationToken.None);

            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.AddAsync(It.IsAny<UserCollectionVolume>()), Times.Once);
            result.Should().NotBeNull();

            result.Success.Should().BeTrue();
            result.createUserCollectionVolumeDto.UserId.Should().Be(command.UserId);
            result.createUserCollectionVolumeDto.MangaId.Should().Be(command.MangaId);
            result.createUserCollectionVolumeDto.VolumeNumber.Should().Be(command.VolumeNumber);
            savedVolumen.Should().NotBeNull();
            savedVolumen.UserId.Should().Be(command.UserId);
            savedVolumen.MangaId.Should().Be(command.MangaId);
            savedVolumen.VolumeNumber.Should().Be(command.VolumeNumber);

        }

        [Fact]
        public async Task Should_Return_Error_When_Collection_Does_Not_Exist()
        {
            var command = new CreateUserCollectionVolumeCommand
            {
                UserId = Guid.NewGuid(),
                MangaId = Guid.NewGuid(),
                VolumeNumber = 1
            };

            _userCollectionRepositoryMock.Setup(repo => repo.GetAsync(command.UserId, command.MangaId))
                .ReturnsAsync((UserCollection?)null);

            var result = await _createUserCollectionVolumeHandler.HandleAsync(command, CancellationToken.None);

            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.AddAsync(It.IsAny<UserCollectionVolume>()), Times.Never);
            _userCollectionRepositoryMock
                .Verify(repo => repo.GetAsync(command.UserId, command.MangaId), Times.Once);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Collection not found for the specified user");

        }

        [Fact]
        public async Task Should_Return_Error_When_Volume_Already_Exists()
        {
            var command = new CreateUserCollectionVolumeCommand
            {
                UserId = Guid.NewGuid(),
                MangaId = Guid.NewGuid(),
                VolumeNumber = 1
            };
            
            var existingVolume = new UserCollectionVolume(command.UserId, command.MangaId, command.VolumeNumber);
            
            _userCollectionRepositoryMock
                .Setup(repo => repo.GetAsync(command.UserId, command.MangaId))
                .ReturnsAsync(new UserCollection(command.UserId, command.MangaId));

            _userCollectionVolumeRepositoryMock
                .Setup(repo => repo.GetUserCollectionVolume(command.UserId, command.MangaId, command.VolumeNumber))
                .ReturnsAsync(existingVolume);
            
            var result = await _createUserCollectionVolumeHandler.HandleAsync(command, CancellationToken.None);
            
            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.AddAsync(It.IsAny<UserCollectionVolume>()), Times.Never);
            
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Existing Volume in user collection");
        }

        [Fact]
        public async Task Should_Return_Error_When_Volume_Number_Is_Invalid()
        {
            var command = new CreateUserCollectionVolumeCommand
            {
                UserId = Guid.NewGuid(),
                MangaId = Guid.NewGuid(),
                VolumeNumber = -1
            };

            var result = await _createUserCollectionVolumeHandler.HandleAsync(command, CancellationToken.None);

            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.AddAsync(It.IsAny<UserCollectionVolume>()), Times.Never);
            
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Error to create user collection volume");
            result.ValidationErrors.Should().ContainSingle()
                .Which.Should().Be("Volume Number must be greater than 0.");
        }
    }
}
