using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Manga.GetMangas
{
    public class GetMangaQueryHandler
    {
        private readonly IMangaRepository _mangaRepository;

        public GetMangaQueryHandler(IMangaRepository mangaRepository)
        {
            _mangaRepository = mangaRepository;
        }

        public async Task<GetMangaQueryResponse> HandleAsync(GetMangaQuery query, CancellationToken cancellationToken) 
        {
            var response = new GetMangaQueryResponse();
            var result = await _mangaRepository.GetAllAsync(query.Title, query.Publisher);

            var listMangaDto = result.Select(x => new GetMangaQueryDto
            {
                MangaId = x.Id,
                Title = x.Title,
                CoverUrl = x.CoverUrl,
                Publisher = x.Publisher,
                Status = x.Status,
                Description = x.Description,
                TotalVolumes = x.TotalVolumes,
            }).ToList();

            response.Success = true;
            response.listGetMangaDto = listMangaDto;

            return response;
        }
    }
}
