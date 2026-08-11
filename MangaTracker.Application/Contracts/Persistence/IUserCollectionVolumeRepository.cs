using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Contracts.Persistence
{
    public interface IUserCollectionVolumeRepository: IAsyncRepository<UserCollectionVolume>
    {
        Task<UserCollectionVolume?> GetUserCollectionVolume(Guid userId, Guid mangaId, int volumeNumber);
        Task<List<UserCollectionVolume>?> GetUserCollectionVolumes(Guid userId, Guid mangaId, int volumeNumber);
    }
}
