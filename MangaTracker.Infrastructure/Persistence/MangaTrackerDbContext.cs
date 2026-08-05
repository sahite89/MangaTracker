using MangaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure.Persistence
{
    public class MangaTrackerDbContext: DbContext
    {
        public MangaTrackerDbContext(DbContextOptions<MangaTrackerDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<UserCollectionVolume> UserCollectionVolumes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MangaTrackerDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
            // Configure your entity mappings here
        }
    }
}
