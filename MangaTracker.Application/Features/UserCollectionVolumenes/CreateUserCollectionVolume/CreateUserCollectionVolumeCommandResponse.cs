using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume
{
    public class CreateUserCollectionVolumeCommandResponse : BaseResponse
    {
        public CreateUserCollectionVolumeCommandResponse() { }

        public CreateUserCollectionVolumeDto createUserCollectionVolumeDto { get; set; }
    }
}
