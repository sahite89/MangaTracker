using MangaTracker.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Manga.GetMangasById
{
    public class GetMangaByIdQueryHandler
    {
        private readonly IMangaRepository _mangaRepository;

        public GetMangaByIdQueryHandler(IMangaRepository mangaRepository)
        {
            _mangaRepository = mangaRepository;
        }

        public async Task<GetMangaByIdQueryResponse> HandleAsync(GetMangaByIdQuery query, CancellationToken cancellationToken) 
        {
            var response = new GetMangaByIdQueryResponse();

            var result = await _mangaRepository.GetByIdAsync(query.MangaId);

            if (result == null)
            {
                response.Success = false;
                response.Message = "Manga not found";
                return response;
            }

            response.Success = true;
            response.MangaByIdDto = new GetMangaByIdDto
            {
                MangaId = result.Id,
                Description = result.Description,
                CoverUrl = result.CoverUrl,
                Publisher = result.Publisher,
                Status = result.Status,
                Title = result.Title,
                TotalVolumes = result.TotalVolumes
            };

            return response;

        }
    }
}
