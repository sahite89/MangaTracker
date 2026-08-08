using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollections.DeleteUserCollection;
using MangaTracker.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MangaTracker.Test.Application.UserCollections.Commands.DeleteUserCollection
{
    public class DeleteUserCollectionCommandHandlerTest
    {
        private readonly Mock<IUserCollectionRepository> _userCollectionRepositoryMock;
        private readonly DeleteUserCollectionCommandHandler _deleteUserCollectionCommandHandler;

        public DeleteUserCollectionCommandHandlerTest()
        {
            _userCollectionRepositoryMock = new Mock<IUserCollectionRepository>();
            _deleteUserCollectionCommandHandler = new DeleteUserCollectionCommandHandler(_userCollectionRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Remove_Collection_When_Collection_Exists()
        {
            
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();
            var existingCollection = new Domain.Entities.UserCollection(userId, mangaId);
            _userCollectionRepositoryMock.Setup(repo => repo.GetAsync(userId, mangaId))
                .ReturnsAsync(existingCollection);

            var userCollectionToDelete = new DeleteUserCollectionCommand(userId, mangaId);

            var response = await _deleteUserCollectionCommandHandler.HandleAsync(userCollectionToDelete, CancellationToken.None);

            Assert.True(response.Success);
            Assert.Equal($"Deleted Collection: {mangaId}", response.Message);
            _userCollectionRepositoryMock.Verify(repo => repo.DeleteAsync(existingCollection), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Error_When_Collection_Does_Not_Exist()
        {

            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            _userCollectionRepositoryMock.Setup(repo => repo.GetAsync(userId, mangaId))
                .ReturnsAsync((UserCollection)null);

            var userCollectionToDelete = new DeleteUserCollectionCommand(userId, mangaId);

            var response = await _deleteUserCollectionCommandHandler.HandleAsync(userCollectionToDelete, CancellationToken.None);

            Assert.False(response.Success);
            Assert.Equal("Collection not found in user", response.Message);
            _userCollectionRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<UserCollection>()), Times.Never);
        }
    }
}
