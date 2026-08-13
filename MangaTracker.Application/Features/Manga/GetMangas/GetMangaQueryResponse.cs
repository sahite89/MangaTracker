using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Manga.GetMangas
{
    public class GetMangaQueryResponse: BaseResponse
    {
        public GetMangaQueryResponse() { }

        public List<GetMangaQueryDto> listGetMangaDto {  get; set; } = new List<GetMangaQueryDto>();
    }
}
