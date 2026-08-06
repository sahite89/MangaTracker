using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.CreateUserCollection
{
    public class CreateUserCollectionDto
    {
        public Guid userId {  get; set; }
        public Guid mangaId { get; set;  }
        public DateTime createdAt { get; set; }
        public ICollection<UserCollectionVolume> volumes { get; set; }
    }
}
