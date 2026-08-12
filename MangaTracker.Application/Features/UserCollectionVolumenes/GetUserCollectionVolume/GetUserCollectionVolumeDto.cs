using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.GetUserCollectionVolume
{
    public class GetUserCollectionVolumeDto
    {
        public List<int> VolumeNumbers { get; set; } = new List<int>();
    }
}
