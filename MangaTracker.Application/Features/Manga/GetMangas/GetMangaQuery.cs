using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Manga.GetMangas
{
    public class GetMangaQuery
    {
        public string? Title { get; set; } = string.Empty;
        public string? Publisher { get; set; } = string.Empty;
    }
}
