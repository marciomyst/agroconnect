using System;
using Agronomia.Domain.Aggregates.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="User"/>.
/// </summary>
/// <remarks>
/// Maps the aggregate to the <c>users</c> table, constrains column lengths, stores enums as strings,
/// and configures the optional relationship to <see cref="Company"/>.
/// </remarks>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User aggregate mapping.
    /// </summary>
    /// <param name="builder">Builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnType("uuid")
            .HasConversion(
                id => Guid.Parse(id),
                id => id.ToString())
            .IsRequired();

        builder.Property(user => user.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.Password)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(user => user.Role)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .IsRequired();
    }
}
