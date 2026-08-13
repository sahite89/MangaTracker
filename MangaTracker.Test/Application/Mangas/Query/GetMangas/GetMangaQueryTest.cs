using FluentAssertions;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.Manga.GetMangas;
using MangaTracker.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MangaTracker.Test.Application.Mangas.Query.GetMangas
{
    public class GetMangaQueryTest
    {
        private readonly Mock<IMangaRepository> _mangaRepository;
        private readonly GetMangaQueryHandler _getMangaQueryHandler;

        public GetMangaQueryTest()
        {
            _mangaRepository = new Mock<IMangaRepository>();
            _getMangaQueryHandler = new GetMangaQueryHandler(_mangaRepository.Object);
        }

        [Fact]
        public async Task Should_Return_All_Manga_Where_Not_Filter_Are_Provided()
        {

            var query = new GetMangaQuery() 
            {
                Title = "",
                Publisher = ""
            };

            var listManga = new List<Manga>()
            {
               new Manga(Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),"One Piece","Planeta",111,"cover.jpg",""),
               new Manga(Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),"Dragon Ball","Planeta",50,"cover.jpg",""),
            };

            _mangaRepository
                .Setup(repo => repo.GetAllAsync(query.Title, query.Publisher))
                .ReturnsAsync(listManga);

            var result = await _getMangaQueryHandler.HandleAsync(query,CancellationToken.None);

            _mangaRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            result.Success.Should().BeTrue();
            result.listGetMangaDto.Should().NotBeNull();
            result.listGetMangaDto.Count.Should().Be(listManga.Count);
            result.listGetMangaDto.Select(x => x.Title).Should().Contain("One Piece", "Dragon Ball");
            
        }

        [Fact]
        public async Task Should_Return_Mangas_Filtered_By_Title()
        {
            var query = new GetMangaQuery()
            {
                Title = "One Piece",
                Publisher = ""
            };

            var listManga = new List<Manga>()
            {
               new Manga(Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),"One Piece","Planeta",111,"cover.jpg",""),
            };

            _mangaRepository
                .Setup(repo => repo.GetAllAsync(query.Title, query.Publisher))
                .ReturnsAsync(listManga);

            var result = await _getMangaQueryHandler.HandleAsync(query, CancellationToken.None);

            _mangaRepository
                .Verify(repo => repo.GetAllAsync("One Piece", It.IsAny<string>()), Times.Once);

            result.Success.Should().BeTrue();
            result.listGetMangaDto.Should().NotBeNull();
            result.listGetMangaDto.Count.Should().Be(listManga.Count);
            result.listGetMangaDto.Select(x => x.Title).Should().Contain("One Piece");
        }
        [Fact]
        public async Task Should_Return_Mangas_Filtered_By_Publisher()
        {
            var query = new GetMangaQuery()
            {
                Title = "",
                Publisher = "Planeta"
            };

            var listManga = new List<Manga>()
            {
               new Manga(Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),"One Piece","Planeta",111,"cover.jpg",""),
            };

            _mangaRepository
                .Setup(repo => repo.GetAllAsync(query.Title, query.Publisher))
                .ReturnsAsync(listManga);

            var result = await _getMangaQueryHandler.HandleAsync(query, CancellationToken.None);

            _mangaRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<string>(), "Planeta"), Times.Once);

            result.Success.Should().BeTrue();
            result.listGetMangaDto.Should().NotBeNull();
            result.listGetMangaDto.Count.Should().Be(listManga.Count);
            result.listGetMangaDto.Select(x => x.Publisher).Should().Contain("Planeta");
        }
        [Fact]
        public async Task Should_Return_Empty_List_When_No_Mangas_Match_Filters()
        {
            var query = new GetMangaQuery()
            {
                Title = "One Piece",
                Publisher = "Planeta"
            };

            _mangaRepository
                .Setup(repo => repo.GetAllAsync(query.Title, query.Publisher))
                .ReturnsAsync(new List<Manga>());

            var result = await _getMangaQueryHandler.HandleAsync(query, CancellationToken.None);

            _mangaRepository
                .Verify(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            result.Success.Should().BeTrue();
            result.listGetMangaDto.Should().NotBeNull();
            result.listGetMangaDto.Should().BeEmpty();
            
        }
    }
}
