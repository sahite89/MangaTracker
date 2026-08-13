using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure.Persistence.Repositories
{
    public class MangaRepository: BaseRepository<Manga>, IMangaRepository
    {
        public MangaRepository(MangaTrackerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Manga>> GetAllAsync(string title, string publisher)
        {
            var query = _dbContext.Mangas.AsNoTracking();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(x => x.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(publisher))
            {
                query = query.Where(x => x.Publisher.Contains(publisher));
            }

            return await query.OrderBy(x => x.Title).ToListAsync();

        }
    }
}
