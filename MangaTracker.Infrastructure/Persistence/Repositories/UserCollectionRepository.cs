using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure.Persistence.Repositories
{
    public class UserCollectionRepository : BaseRepository<UserCollection>, IUserCollectionRepository
    {
        public UserCollectionRepository(MangaTrackerDbContext dbContext): base(dbContext) {}

        public async Task<List<UserCollection>> GetAllCollectionByUserIdAsync(Guid userId)
        {
            return await _dbContext.UserCollections
                .Include(x => x.Volumes)
                .Include(x => x.Manga)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserCollection?> GetAsync(Guid userId, Guid mangaId)
        {
            return await _dbContext.UserCollections
                .Include(x => x.Volumes)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MangaId == mangaId);
        }
    }
}
