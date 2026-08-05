using MangaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure.Persistence.Configurations
{
    public class UserCollectionConfiguration : IEntityTypeConfiguration<UserCollection>
    {
        public void Configure(EntityTypeBuilder<UserCollection> builder)
        {
            builder.ToTable("UserCollections");

            builder.HasKey(x => new
            {
                x.UserId,
                x.MangaId
            });

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.MangaId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Manga)
                .WithMany()
                .HasForeignKey(x => x.MangaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Volumes)
                .WithOne(x => x.UserCollection)
                .HasForeignKey(x => new
                {
                    x.UserId,
                    x.MangaId
                })
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
