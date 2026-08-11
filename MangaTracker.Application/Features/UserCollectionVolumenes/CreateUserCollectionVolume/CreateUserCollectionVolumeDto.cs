using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume
{
    public class CreateUserCollectionVolumeDto
    {
        public Guid UserId { get; set; }
        public Guid MangaId { get; set; }
        public int VolumeNumber { get; set; }
    }
}
