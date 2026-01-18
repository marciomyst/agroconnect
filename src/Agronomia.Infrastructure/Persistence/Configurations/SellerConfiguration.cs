using Agronomia.Domain.Aggregates.Sellers;
using Agronomia.Domain.Aggregates.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Agronomia.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="Seller"/>.
/// </summary>
internal sealed class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.ToTable("sellers");

        builder.HasKey(seller => seller.Id);

        builder.Property(seller => seller.Id)
            .HasColumnType("uuid")
            .HasConversion(
                id => Guid.Parse(id),
                id => id.ToString())
            .IsRequired();

        builder.Property(seller => seller.LegalName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(seller => seller.TradeName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(seller => seller.Document)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(seller => seller.StateRegistration)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(seller => seller.ContactEmail)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(seller => seller.ContactPhone)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(seller => seller.ResponsibleName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(seller => seller.ZipCode)
            .HasMaxLength(12)
            .IsRequired();

        builder.Property(seller => seller.Street)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(seller => seller.Number)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(seller => seller.City)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(seller => seller.State)
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(seller => seller.Complement)
            .HasMaxLength(120);

        builder.Property(seller => seller.LogoUrl)
            .HasMaxLength(512);

        builder.Property(seller => seller.CreatedAt)
            .IsRequired();

        builder
            .HasMany(seller => seller.Managers)
            .WithMany(user => user.ManagedSellers)
            .UsingEntity<Dictionary<string, object>>(
                "seller_users",
                right => right
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_seller_users_users_UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Seller>()
                    .WithMany()
                    .HasForeignKey("SellerId")
                    .HasConstraintName("FK_seller_users_sellers_SellerId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("seller_users");
                    join.HasKey("SellerId", "UserId");

                    join.Property<string>("SellerId")
                        .HasColumnType("uuid")
                        .HasConversion(
                            id => Guid.Parse(id),
                            id => id.ToString())
                        .IsRequired();

                    join.Property<string>("UserId")
                        .HasColumnType("uuid")
                        .HasConversion(
                            id => Guid.Parse(id),
                            id => id.ToString())
                        .IsRequired();
                });
    }
}
