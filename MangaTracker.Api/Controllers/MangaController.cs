using MangaTracker.Api.Models.Manga;
using MangaTracker.Application.Features.Manga.GetMangas;
using MangaTracker.Application.Features.Manga.GetMangasById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaController : ControllerBase
    {
        private readonly GetMangaQueryHandler _getMangaQueryHandler;
        private readonly GetMangaByIdQueryHandler _getMangaByIdQueryHandler;

        public MangaController(GetMangaQueryHandler getMangaQueryHandler, GetMangaByIdQueryHandler getMangaByIdQueryHandler)
        {
            _getMangaQueryHandler = getMangaQueryHandler;
            _getMangaByIdQueryHandler = getMangaByIdQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMangas([FromQuery] GetAllMangaRequest request ,CancellationToken cancellationToken)
        {
            var query = new GetMangaQuery
            {
                Title = request.Title,
                Publisher = request.Publisher
            };

            var listAllManga = await _getMangaQueryHandler.HandleAsync(query, cancellationToken);

            return Ok(listAllManga);
        }

        [HttpGet("{mangaId}")]
        public async Task<IActionResult> GetMangaById(Guid mangaId, CancellationToken cancelationToken)
        {
            var query = new GetMangaByIdQuery
            {
                MangaId = mangaId
            };

            var response = await _getMangaByIdQueryHandler.HandleAsync(query, cancelationToken);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}
