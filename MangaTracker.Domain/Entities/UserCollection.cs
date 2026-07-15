using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Domain.Entities
{
    public class UserCollection
    {
        public Guid UserId { get; set; }
        public Guid MangaId { get; set; }
        public int OwnedVolumes { get; set; }
        public bool Completed { get; set; }
    }
}
