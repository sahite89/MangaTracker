using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.GetUserCollectionVolume
{
    public class GetUserCollectionVolumeQueryResponse: BaseResponse
    {
        public GetUserCollectionVolumeQueryResponse() { }

        public GetUserCollectionVolumeDto getUserCollectionVolumeDto { get; set; }
    }
}
