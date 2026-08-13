using MangaTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Manga.GetMangasById
{
    public class GetMangaByIdDto
    {
        public Guid MangaId { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int TotalVolumes { get; set; }
        public string CoverUrl { get; set; }
        public MangaStatus Status { get; set; }
        public string? Description { get; set; }
    }
}
