using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Domain.Entities
{
    public class UserCollectionVolume
    {
        public Guid UserId { get; private set; }
        public Guid MangaId { get; private set; }
        public int VolumeNumber { get; private set; }
        public UserCollection UserCollection { get; private set; }

        private UserCollectionVolume()
        {
        }

        public UserCollectionVolume(Guid userId, Guid mangaId, int volumeNumber)
        {
            UserId = userId;
            MangaId = mangaId;
            VolumeNumber = volumeNumber;
        }
    }
}
