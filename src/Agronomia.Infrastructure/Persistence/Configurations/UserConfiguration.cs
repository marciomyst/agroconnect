using Agronomia.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="User"/>.
/// </summary>
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
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(user => user.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(user => user.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();
        });

        builder.Navigation(user => user.Email)
            .IsRequired();

        builder.HasIndex(user => user.Email.Value)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.CreatedAtUtc)
            .IsRequired();
    }
}
