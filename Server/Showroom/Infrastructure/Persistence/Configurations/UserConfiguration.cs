﻿
using Skynet.Showroom.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Skynet.Showroom.Infrastructure.Persistence.Configurations;

class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasQueryFilter(i => i.Deleted == null && i.Hidden == false);

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.LastModifiedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.DeletedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
