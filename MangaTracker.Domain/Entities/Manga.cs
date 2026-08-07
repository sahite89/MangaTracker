namespace MangaTracker.Domain.Entities
{
    public class Manga
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Publisher { get; private set; }
        public int TotalVolumes { get; private set; }
        public string CoverUrl { get; private set; }

        private Manga() { }

        public Manga(Guid id, string title, string publisher, int totalVolumes, string coverUrl)
        {
            Id = id;
            Title = title;
            Publisher = publisher;
            TotalVolumes = totalVolumes;
            CoverUrl = coverUrl;
        }

    }
}