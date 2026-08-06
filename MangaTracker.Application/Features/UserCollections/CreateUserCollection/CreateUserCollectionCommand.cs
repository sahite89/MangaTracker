using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.CreateUserCollection
{
    public class CreateUserCollectionCommand
    {
        public Guid UserId { get; set; }
        public Guid MangaId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
