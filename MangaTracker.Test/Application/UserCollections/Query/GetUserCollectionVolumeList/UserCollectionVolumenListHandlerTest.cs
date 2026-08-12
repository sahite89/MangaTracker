using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollectionVolumenes.GetUserCollectionVolume;
using MangaTracker.Domain.Entities;
using Moq;
using Xunit;

namespace MangaTracker.Test.Application.UserCollections.Query.GetUserCollectionVolumeList
{
    public class UserCollectionVolumenListHandlerTest
    {
        private readonly Mock<IUserCollectionVolumeRepository> _mockUserCollectionVolumeRepository;
        private readonly Mock<IUserCollectionRepository> _mockUserCollectionRepository;
        private readonly GetUserCollectionVolumeQueryHandler _getUserCollectionVolumeListQueryHandler;

        public UserCollectionVolumenListHandlerTest()
        {
            _mockUserCollectionVolumeRepository = new Mock<IUserCollectionVolumeRepository>();
            _mockUserCollectionRepository = new Mock<IUserCollectionRepository>();
            _getUserCollectionVolumeListQueryHandler = new GetUserCollectionVolumeQueryHandler(_mockUserCollectionVolumeRepository.Object, _mockUserCollectionRepository.Object);
        }

        [Fact]
        public async Task Should_Return_Volumes_When_collection_Exists() { 
            
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var existingCollection = new UserCollection(userId, mangaId);
            
            
            _mockUserCollectionRepository.Setup(x => x.GetAsync(userId, mangaId))
                .ReturnsAsync(existingCollection);

            _mockUserCollectionVolumeRepository.Setup(x => x.GetUserCollectionVolumes(userId, mangaId))
                .ReturnsAsync(new List<UserCollectionVolume> 
                                { 
                                    new UserCollectionVolume(userId,mangaId,1), 
                                    new UserCollectionVolume(userId,mangaId,2), 
                                    new UserCollectionVolume(userId,mangaId,3) 
                                });

            var query = new GetUserCollectionVolumeQuery
            {
                UserId = userId,
                MangaId = mangaId
            };
            var result = await _getUserCollectionVolumeListQueryHandler.HandleAsync(query, CancellationToken.None);

            _mockUserCollectionRepository
                .Verify(repo => repo.GetAsync(userId, mangaId), Times.Once);
            _mockUserCollectionVolumeRepository
                .Verify(repo => repo.GetUserCollectionVolumes(userId, mangaId), Times.Once);
            result.Success.Should().BeTrue();
            result.getUserCollectionVolumeDto.VolumeNumbers.Should().NotBeNull();
            result.getUserCollectionVolumeDto.VolumeNumbers.Should().HaveCount(3);
            result.getUserCollectionVolumeDto.VolumeNumbers.Should().Equal(1, 2, 3);
        }

        [Fact]
        public async Task Should_Return_Error_When_Collection_Does_Not_Exist()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            _mockUserCollectionRepository.Setup(x => x.GetAsync(userId, mangaId))
                .ReturnsAsync((UserCollection?)null);

            var query = new GetUserCollectionVolumeQuery
            {
                UserId = userId,
                MangaId = mangaId
            };
            var result = await _getUserCollectionVolumeListQueryHandler.HandleAsync(query, CancellationToken.None);
            
            _mockUserCollectionRepository
                .Verify(repo => repo.GetAsync(userId, mangaId), Times.Once);
            _mockUserCollectionVolumeRepository
                .Verify(repo => repo.GetUserCollectionVolumes(userId, mangaId), Times.Never);
            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User collection not found.");
            result.getUserCollectionVolumeDto.Should().BeNull();
        }

        [Fact]
        public async Task Should_Return_Empty_List_When_Collection_Has_No_Volumes()
        {
            var userId = Guid.NewGuid();
            var mangaId = Guid.NewGuid();

            var existingCollection = new UserCollection(userId, mangaId);

            _mockUserCollectionRepository.Setup(x => x.GetAsync(userId, mangaId))
                .ReturnsAsync(existingCollection);

            _mockUserCollectionVolumeRepository
                .Setup(x => x.GetUserCollectionVolumes(userId, mangaId))
                .ReturnsAsync(new List<UserCollectionVolume>());

            var query = new GetUserCollectionVolumeQuery
            {
                UserId = userId,
                MangaId = mangaId
            };
            var result = await _getUserCollectionVolumeListQueryHandler.HandleAsync(query, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.Should().NotBeNull();
            result.getUserCollectionVolumeDto.VolumeNumbers.Should().NotBeNull();
            result.getUserCollectionVolumeDto.VolumeNumbers.Should().BeEmpty();

        }
    }
}
