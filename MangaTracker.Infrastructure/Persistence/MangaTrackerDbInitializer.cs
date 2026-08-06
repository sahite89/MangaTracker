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

                new Manga
                {
                    Id = Guid.NewGuid(),
                    Title = "One Piece",
                    Publisher = "test",
                    CoverUrl = "",
                    TotalVolumes = 50,
                },
                new Manga
                {
                    Id = Guid.NewGuid(),
                    Title = "Dragon Ball",
                    Publisher = "test",
                    CoverUrl = "",
                    TotalVolumes = 100,
                }

                );

            await context.SaveChangesAsync();
        }
    }
}
