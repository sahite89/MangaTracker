using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.Manga.GetMangasById;
using MangaTracker.Domain.Entities;
using Moq;
using Xunit;

namespace MangaTracker.Test.Application.Mangas.Query.GetMangasById
{
    public class GetMangaByIdQueryTest
    {
        private readonly Mock<IMangaRepository> _mangaRepository;
        private readonly GetMangaByIdQueryHandler _getMangaByIdQueryHandler;

        public GetMangaByIdQueryTest()
        {
            _mangaRepository = new Mock<IMangaRepository>();
            _getMangaByIdQueryHandler = new GetMangaByIdQueryHandler(_mangaRepository.Object);
        }

        [Fact]
        public async Task Should_Return_Manga_When_Id_is_Correct()
        {
            var mangaId = Guid.NewGuid();
            var manga = new Manga(mangaId, "One Piece", "Planeta", 150, "", "");
            var query = new GetMangaByIdQuery()
            {
                MangaId = mangaId
            };

            _mangaRepository
                .Setup(repo => repo.GetByIdAsync(mangaId))
                .ReturnsAsync(manga);

            var result = await _getMangaByIdQueryHandler.HandleAsync(query, CancellationToken.None);

            _mangaRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            result.Success.Should().BeTrue();
            result.MangaByIdDto.Should().NotBeNull();
            result.MangaByIdDto.MangaId.Should().Be(mangaId);

        }

        [Fact]
        public async Task Should_Return_Error__When_MangaId_is_Incorrect()
        {
            var mangaId = Guid.NewGuid();
            var query = new GetMangaByIdQuery()
            {
                MangaId = mangaId
            };

            _mangaRepository
                .Setup(repo => repo.GetByIdAsync(mangaId))
                .ReturnsAsync((Manga)null);

            var result = await _getMangaByIdQueryHandler.HandleAsync(query, CancellationToken.None);

            _mangaRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);

            result.Success.Should().BeFalse();
            result.Message.Contains("Manga not found");
            result.MangaByIdDto.Should().BeNull();

        }
    }
}
