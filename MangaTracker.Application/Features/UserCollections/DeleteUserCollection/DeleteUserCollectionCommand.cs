using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.DeleteUserCollection
{
    public class DeleteUserCollectionCommand
    {
        public Guid UserId { get; set; }
        public Guid MangaId { get; private set; }

        private DeleteUserCollectionCommand() { }

        public DeleteUserCollectionCommand(Guid userId, Guid mangaId)
        {
            UserId = userId;
            MangaId = mangaId;
        }
    }

    
}
