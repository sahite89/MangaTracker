using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollections.GetUserCollectionList;
using MangaTracker.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Xunit;

namespace MangaTracker.Test.Application.UserCollections.Query.GetUserCollectionList
{
    public class UserCollectionListQueryHandlerTest
    {
        private readonly Mock<IUserCollectionRepository> _userCollectionRepository;
        private readonly GetUserCollectionListQueryHandler _userCollectionListHandler;

        public UserCollectionListQueryHandlerTest()
        {
            _userCollectionRepository = new Mock<IUserCollectionRepository>();
            _userCollectionListHandler = new GetUserCollectionListQueryHandler(_userCollectionRepository.Object);
        }

        [Fact]
        public async Task Should_Return_Empty_List_When_User_Has_No_Collection() 
        {
            var userId = Guid.NewGuid();

            _userCollectionRepository.Setup(x => x.GetAllCollectionByUserIdAsync(userId))
                .ReturnsAsync([]);

            var query = new GetUserCollectionListQuery
            {
                UserId = userId
            };

            var result = await _userCollectionListHandler.HandleAsync(query, CancellationToken.None);

            result.listUserCollections.Should().NotBeNull();
            result.listUserCollections.Should().BeEmpty();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("No collections found for the user.");
        }

        [Fact]
        public async Task Should_Return_UserCollections_When_Collections_Exist() 
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var manga = new Manga(mangaId,"One Piece","Planeta",111,"cover.jpg");
            var collection = new UserCollection(userId, mangaId);

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Manga))!
                .SetValue(collection, manga);

            _userCollectionRepository
                .Setup(x => x.GetAllCollectionByUserIdAsync(userId))
                .ReturnsAsync(new List<UserCollection> { collection });


            var query = new GetUserCollectionListQuery
            {
                UserId = userId
            };

            var result = await _userCollectionListHandler.HandleAsync(query, CancellationToken.None);

            result.listUserCollections.Should().NotBeNull();
            result.listUserCollections.Should().HaveCount(1);

        }

        [Fact]
        public async Task Should_Map_Manga_Information_Correctly()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var manga = new Manga(mangaId, "One Piece", "Planeta", 111, "cover.jpg");
            var collection = new UserCollection(userId, mangaId);

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Manga))!
                .SetValue(collection, manga);

            _userCollectionRepository
                .Setup(x => x.GetAllCollectionByUserIdAsync(userId))
                .ReturnsAsync(new List<UserCollection> { collection });


            var query = new GetUserCollectionListQuery
            {
                UserId = userId
            };

            var result = await _userCollectionListHandler.HandleAsync(query, CancellationToken.None);

            result.listUserCollections.Should().NotBeNull();
            result.listUserCollections.Should().HaveCount(1);
            Assert.Equal(manga.Title, result.listUserCollections[0].Title);
            Assert.Equal(manga.TotalVolumes, result.listUserCollections[0].TotalVolumes);
            Assert.Equal(manga.CoverUrl, result.listUserCollections[0].CoverUrl);
            Assert.Equal(manga.Publisher, result.listUserCollections[0].Publisher);
        }

        [Fact]
        public async Task Should_Return_Correct_OwnedVolumes() 
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();
            var manga = new Manga(mangaId, "One Piece", "Planeta", 111, "cover.jpg");
            var collection = new UserCollection(userId, mangaId);

            var userCollectionVolume = new List<UserCollectionVolume>
                {
                    new UserCollectionVolume(userId, mangaId, 1),
                    new UserCollectionVolume(userId, mangaId, 2),
                    new UserCollectionVolume(userId, mangaId, 3),
                    new UserCollectionVolume(userId, mangaId, 5),
                    new UserCollectionVolume(userId, mangaId, 10)
                };

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Manga))!
                .SetValue(collection, manga);

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Volumes))!
                .SetValue(collection, userCollectionVolume);

            _userCollectionRepository
               .Setup(x => x.GetAllCollectionByUserIdAsync(userId))
               .ReturnsAsync(new List<UserCollection> { collection });

            var query = new GetUserCollectionListQuery
            {
                UserId = userId
            };

            var result = await _userCollectionListHandler.HandleAsync(query, CancellationToken.None);

            result.listUserCollections.Should().NotBeNull();
            result.listUserCollections.Should().HaveCount(1);
            result.listUserCollections[0].Volumes.Should().HaveCount(userCollectionVolume.Count);
            Assert.Equal(1, result.listUserCollections[0].Volumes[0]);
            Assert.Equal(10, result.listUserCollections[0].Volumes[4]);
        }

        [Fact]
        public async Task Should_Return_Correct_CompletionPercentage()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();
            var manga = new Manga(mangaId, "One Piece", "Planeta", 100, "cover.jpg");
            var collection = new UserCollection(userId, mangaId);

            var userCollectionVolume = new List<UserCollectionVolume>
                {
                    new UserCollectionVolume(userId, mangaId, 1),
                    new UserCollectionVolume(userId, mangaId, 2),
                    new UserCollectionVolume(userId, mangaId, 3),
                    new UserCollectionVolume(userId, mangaId, 5),
                    new UserCollectionVolume(userId, mangaId, 10)
                };

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Manga))!
                .SetValue(collection, manga);

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Volumes))!
                .SetValue(collection, userCollectionVolume);

            _userCollectionRepository
               .Setup(x => x.GetAllCollectionByUserIdAsync(userId))
               .ReturnsAsync(new List<UserCollection> { collection });

            var query = new GetUserCollectionListQuery
            {
                UserId = userId
            };

            var result = await _userCollectionListHandler.HandleAsync(query, CancellationToken.None);

            result.listUserCollections.Should().NotBeNull();
            result.listUserCollections.Should().HaveCount(1);
            result.listUserCollections[0].Volumes.Should().HaveCount(userCollectionVolume.Count);
            result.listUserCollections[0].CompletionPercentage.Should().Be(5);
        }

        [Fact]
        public async Task Should_Return_Volumes_Ordered()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();
            var manga = new Manga(mangaId, "One Piece", "Planeta", 100, "cover.jpg");
            var collection = new UserCollection(userId, mangaId);

            var userCollectionVolume = new List<UserCollectionVolume>
                {
                    new UserCollectionVolume(userId, mangaId, 1),
                    new UserCollectionVolume(userId, mangaId, 4),
                    new UserCollectionVolume(userId, mangaId, 8),
                    new UserCollectionVolume(userId, mangaId, 5),
                    new UserCollectionVolume(userId, mangaId, 20)
                };

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Manga))!
                .SetValue(collection, manga);

            typeof(UserCollection)
                .GetProperty(nameof(UserCollection.Volumes))!
                .SetValue(collection, userCollectionVolume);

            _userCollectionRepository
               .Setup(x => x.GetAllCollectionByUserIdAsync(userId))
               .ReturnsAsync(new List<UserCollection> { collection });

            var query = new GetUserCollectionListQuery
            {
                UserId = userId
            };

            var result = await _userCollectionListHandler.HandleAsync(query, CancellationToken.None);

            result.listUserCollections.Should().NotBeNull();
            result.listUserCollections.Should().HaveCount(1);
            result.listUserCollections[0].Volumes.Should().HaveCount(userCollectionVolume.Count);
            Assert.Equal(1, result.listUserCollections[0].Volumes[0]);
            Assert.Equal(5, result.listUserCollections[0].Volumes[2]);
            Assert.Equal(8, result.listUserCollections[0].Volumes[3]);
        }
    }
}
