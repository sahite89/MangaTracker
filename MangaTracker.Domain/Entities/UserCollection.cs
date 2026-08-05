using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Domain.Entities
{
    public class UserCollection
    {
        public Guid UserId { get; private set; }
        public Guid MangaId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public User User { get; private set; }
        public Manga Manga { get; private set; }
        public ICollection<UserCollectionVolume> Volumes { get; private set; }
            = new List<UserCollectionVolume>();

        private UserCollection() { }

        public UserCollection(Guid userId, Guid mangaId)
        {
            UserId = userId;
            MangaId = mangaId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
