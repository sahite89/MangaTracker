using System;
using System.Collections.Generic;
using System.Text;
using MangaTracker.Domain.Entities;

namespace MangaTracker.Infrastructure.Persistence
{
    public static class MangaTrackerDbInitializer
    {
        public static async Task SeedAsync(MangaTrackerDbContext context)
        {
            if (context.Mangas.Any())
            {
                return;
            }

            context.Mangas.AddRange(
                new Manga(Guid.NewGuid(), "One Piece", "test", 50, "",""),
                new Manga(Guid.NewGuid(), "Dragon Ball", "test", 100, "",""),
                new Manga(Guid.NewGuid(), "Naruto", "test", 20, "", ""),
                new Manga(Guid.NewGuid(), "Haikyuu", "test", 36, "", "")
                );

            await context.SaveChangesAsync();
        }
    }
}
