using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollectionVolumenes.DeleteUserCollectionVolume;
using MangaTracker.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace MangaTracker.Test.Application.UserCollections.Commands.DeleteUserCollectionVolume
{
    public class DeleteUserCollectionCommandHandlerTest
    {
        private readonly Mock<IUserCollectionVolumeRepository> _userCollectionVolumeRepositoryMock;
        private readonly DeleteUserCollectionVolumeCommandHandler _deleteUserCollectionVolumeCommandHandler;


        public DeleteUserCollectionCommandHandlerTest()
        {
            _userCollectionVolumeRepositoryMock = new Mock<IUserCollectionVolumeRepository>();
            _deleteUserCollectionVolumeCommandHandler = new DeleteUserCollectionVolumeCommandHandler(_userCollectionVolumeRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Delete_Volume_When_Request_Is_Valid()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var collectionVolumeList = new List<UserCollectionVolume> { new UserCollectionVolume(userId, mangaId, 1),
                                                                         new UserCollectionVolume(userId, mangaId, 2),
                                                                         new UserCollectionVolume(userId, mangaId, 3)
                                                                        };
            var volume = new UserCollectionVolume(userId, mangaId, 1);

            _userCollectionVolumeRepositoryMock
                .Setup(repo => repo.GetUserCollectionVolumes(userId, mangaId))
                .ReturnsAsync(collectionVolumeList);

            _userCollectionVolumeRepositoryMock
                .Setup(repo => repo.GetUserCollectionVolume(userId, mangaId, 1))
                .ReturnsAsync(volume);

            var userCollectionToDelete = new DeleteUserCollectionVolumeCommand(userId, mangaId, 1);

            var response = await _deleteUserCollectionVolumeCommandHandler.HandleAsync(userCollectionToDelete, CancellationToken.None);

            _userCollectionVolumeRepositoryMock.Verify(
                        repo => repo.GetUserCollectionVolumes(userId, mangaId), Times.Once);

            _userCollectionVolumeRepositoryMock.Verify(
                        repo => repo.GetUserCollectionVolume(userId, mangaId, 1), Times.Once);

            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.DeleteAsync(volume), Times.Once);

            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            Assert.Equal($"Deleted volume succeeded: {volume.VolumeNumber}", response.Message);

        }

        [Fact]
        public async Task Should_Return_Error_When_Collection_Does_Not_Exist()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var volume = new UserCollectionVolume(userId, mangaId, 1);

            _userCollectionVolumeRepositoryMock
                .Setup(repo => repo.GetUserCollectionVolumes(userId, mangaId))
                .ReturnsAsync(new List<UserCollectionVolume>());

            var userCollectionToDelete = new DeleteUserCollectionVolumeCommand(userId, mangaId, 1);

            var response = await _deleteUserCollectionVolumeCommandHandler.HandleAsync(userCollectionToDelete, CancellationToken.None);

            _userCollectionVolumeRepositoryMock.Verify(
                        repo => repo.GetUserCollectionVolumes(userId, mangaId), Times.Once);

            _userCollectionVolumeRepositoryMock.Verify(
                        repo => repo.GetUserCollectionVolume(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<int>()),Times.Never);

            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.DeleteAsync(It.IsAny<UserCollectionVolume>()), Times.Never);

            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            Assert.Equal($"Collection not found.", response.Message);
        }

        [Fact]
        public async Task Should_Return_Error_When_Volume_Does_Not_Exist()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var collectionVolumeList = new List<UserCollectionVolume> { new UserCollectionVolume(userId, mangaId, 1),
                                                                         new UserCollectionVolume(userId, mangaId, 5),
                                                                         new UserCollectionVolume(userId, mangaId, 7)
                                                                        };
            _userCollectionVolumeRepositoryMock
                .Setup(repo => repo.GetUserCollectionVolumes(userId, mangaId))
                .ReturnsAsync(collectionVolumeList);

            _userCollectionVolumeRepositoryMock
                .Setup(repo => repo.GetUserCollectionVolume(userId, mangaId, 2))
                .ReturnsAsync((UserCollectionVolume)null);

            var userCollectionToDelete = new DeleteUserCollectionVolumeCommand(userId, mangaId, 2);

            var response = await _deleteUserCollectionVolumeCommandHandler.HandleAsync(userCollectionToDelete, CancellationToken.None);

            _userCollectionVolumeRepositoryMock.Verify(
                        repo => repo.GetUserCollectionVolumes(userId, mangaId), Times.Once);

            _userCollectionVolumeRepositoryMock.Verify(
                        repo => repo.GetUserCollectionVolume(userId, mangaId, 2), Times.Once);

            _userCollectionVolumeRepositoryMock
                .Verify(repo => repo.DeleteAsync(It.IsAny<UserCollectionVolume>()), Times.Never);

            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            Assert.Equal($"Volume not found in collection.", response.Message);
        }
    }
}
