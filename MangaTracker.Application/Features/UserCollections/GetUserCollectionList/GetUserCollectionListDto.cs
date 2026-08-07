using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.GetUserCollectionList
{
    public class GetUserCollectionListDto
    {
        public Guid MangaId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public int OwnedVolumes { get; set; }
        public int TotalVolumes { get; set; }
        public decimal CompletionPercentage { get; set; }
        public List<int> Volumes { get; set; } = [];
    }
}
