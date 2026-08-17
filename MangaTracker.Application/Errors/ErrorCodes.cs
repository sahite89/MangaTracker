using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Errors
{
    public enum ErrorCode
    {
        None,
        ValidationError,
        UserAlreadyExists,
        InvalidCredentials,
        MangaNotFound,
        CollectionNotFound,
        CollectionAlreadyExists,
        VolumeAlreadyExists,
        InvalidVolumeNumber,
        VolumeNotFound
    }
}
