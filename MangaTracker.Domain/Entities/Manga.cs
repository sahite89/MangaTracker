namespace MangaTracker.Domain.Entities
{
    public class Manga
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int TotalVolumes { get; set; }
        public string CoverUrl { get; set; }
    }
}