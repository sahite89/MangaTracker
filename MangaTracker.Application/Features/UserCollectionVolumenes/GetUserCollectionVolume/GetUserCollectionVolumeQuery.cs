using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.GetUserCollectionVolume
{
    public class GetUserCollectionVolumeQuery
    {
        public Guid UserId { get; set; }
        public Guid MangaId { get; set; }
    }
}
