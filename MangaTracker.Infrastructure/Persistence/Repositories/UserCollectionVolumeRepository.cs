using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure.Persistence.Repositories
{
    public class UserCollectionVolumeRepository : BaseRepository<UserCollectionVolume>, IUserCollectionVolumeRepository
    {
        public UserCollectionVolumeRepository(MangaTrackerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserCollectionVolume?> GetUserCollectionVolume(Guid userId,Guid mangaId,int volumeNumber)
        {
            return await _dbContext.UserCollectionVolumes
                .FirstOrDefaultAsync(v =>
                    v.UserId == userId &&
                    v.MangaId == mangaId &&
                    v.VolumeNumber == volumeNumber);
        }
        public async Task<List<UserCollectionVolume>?> GetUserCollectionVolumes(Guid userId, Guid mangaId, int volumeNumber)
        {
            return await _dbContext.UserCollectionVolumes
                .Where(v => v.UserId == userId && v.MangaId == mangaId && v.VolumeNumber == volumeNumber)
                .ToListAsync();

        }
    }
}
