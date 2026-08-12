using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.DeleteUserCollectionVolume
{
    public class DeleteUserCollectionVolumeCommand
    {
        public Guid UserId {  get; private set; }
        public Guid MangaId { get; private set; }
        public int VolumeNumber { get; private set; }

        private DeleteUserCollectionVolumeCommand() { }

        public DeleteUserCollectionVolumeCommand(Guid userId, Guid mangaId, int volumeNumber)
        {
            UserId = userId;
            MangaId = mangaId;
            VolumeNumber = volumeNumber;
        }
    }
}
