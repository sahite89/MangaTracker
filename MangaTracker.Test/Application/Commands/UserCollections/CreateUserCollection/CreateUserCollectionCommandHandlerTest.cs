using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollections.CreateUserCollection;
using MangaTracker.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MangaTracker.Test.Application.Commands.UserCollections.CreateUserCollection
{
    public class CreateUserCollectionCommandHandlerTest
    {
        private readonly Mock<IUserCollectionRepository> _userCollectionRepository;
        private readonly CreateUserCollectionCommandHandler _createUserCollectionHandler;

        public CreateUserCollectionCommandHandlerTest()
        {
            _userCollectionRepository = new Mock<IUserCollectionRepository>();
            _createUserCollectionHandler = new CreateUserCollectionCommandHandler(_userCollectionRepository.Object);
        }


        [Fact]
        public async Task Should_Create_Collection_When_Not_Exist()
        {
            var newCollectionToInsert = new CreateUserCollectionCommand
            {
                UserId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
                MangaId = Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),
                CreatedAt = DateTime.UtcNow,
            };

            _userCollectionRepository
                .Setup(repo => repo.GetAsync(newCollectionToInsert.UserId, newCollectionToInsert.MangaId))
                .ReturnsAsync((UserCollection?)null);

            _userCollectionRepository
                .Setup(repo => repo.AddAsync(It.IsAny<UserCollection>()))
                .ReturnsAsync((UserCollection userCollection) => userCollection);

            var createdCollection = await _createUserCollectionHandler.HandleAsync(newCollectionToInsert, CancellationToken.None);

            _userCollectionRepository
                .Verify(x => x.AddAsync(It.IsAny<UserCollection>()), Times.Once());

            createdCollection.Success.Should().BeTrue();
            createdCollection.createUserCollectionDto.Should().NotBeNull();
            Assert.Equal(createdCollection.createUserCollectionDto.userId, newCollectionToInsert.UserId);
            Assert.Equal(createdCollection.createUserCollectionDto.mangaId, newCollectionToInsert.MangaId);
        }

        [Fact]
        public async Task Should_Return_Error_When_Collection_Already_Exist()
        {
            var newCollectionToInsert = new CreateUserCollectionCommand
            {
                UserId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
                MangaId = Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),
                CreatedAt = DateTime.UtcNow,
            };

            var existingCollection = new UserCollection(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"));
            
            _userCollectionRepository
                .Setup(repo => repo.GetAsync(newCollectionToInsert.UserId, newCollectionToInsert.MangaId))
                .ReturnsAsync(existingCollection);

            var createdCollection = await _createUserCollectionHandler.HandleAsync(newCollectionToInsert, CancellationToken.None);

            _userCollectionRepository
               .Verify(x => x.AddAsync(It.IsAny<UserCollection>()), Times.Never());

            createdCollection.Success.Should().BeFalse();
            createdCollection.createUserCollectionDto.Should().BeNull();
            createdCollection.Message.Contains("Existing Collection in user");
        }
    }
}
