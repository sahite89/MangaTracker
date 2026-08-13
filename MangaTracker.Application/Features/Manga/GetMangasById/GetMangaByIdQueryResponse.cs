using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Manga.GetMangasById
{
    public class GetMangaByIdQueryResponse : BaseResponse
    {
        public GetMangaByIdQueryResponse() { }

        public GetMangaByIdDto MangaByIdDto { get; set; }
    }
}
