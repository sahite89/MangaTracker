using MangaTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure.Persistence.Configurations
{
    public class UserCollectionVolumeConfiguration : IEntityTypeConfiguration<UserCollectionVolume>
    {
        public void Configure(EntityTypeBuilder<UserCollectionVolume> builder)
        {
            builder.ToTable("UserCollectionVolumes");
            builder.HasKey(x => new
            {
                x.UserId,
                x.MangaId,
                x.VolumeNumber
            });
            builder.Property(e => e.UserId)
                .IsRequired();
            builder.Property(e => e.MangaId)
                .IsRequired();
            builder.Property(e => e.VolumeNumber)
                .IsRequired();

        }

    }
}
