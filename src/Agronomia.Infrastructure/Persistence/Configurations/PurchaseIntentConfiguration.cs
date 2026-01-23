using Agronomia.Domain.Catalog.Products;
using Agronomia.Domain.Catalog.SellerProducts;
using Agronomia.Domain.Orders.PurchaseIntents;
using Agronomia.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class PurchaseIntentConfiguration : IEntityTypeConfiguration<PurchaseIntent>
{
    public void Configure(EntityTypeBuilder<PurchaseIntent> builder)
    {
        builder.ToTable("purchase_intents");

        builder.HasKey(intent => intent.Id);

        builder.Property(intent => intent.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(intent => intent.FarmId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(intent => intent.SellerId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(intent => intent.ProductId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(intent => intent.SellerProductId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(intent => intent.Quantity)
            .HasConversion(
                quantity => quantity.Value,
                value => Quantity.Create(value))
            .HasColumnName("Quantity")
            .HasColumnType("numeric")
            .IsRequired();

        builder.Property(intent => intent.Notes)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(intent => intent.Status)
            .HasConversion<string>()
            .HasColumnName("Status")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(intent => intent.RequestedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(intent => intent.UpdatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasIndex(intent => intent.FarmId);
        builder.HasIndex(intent => intent.SellerId);
        builder.HasIndex(intent => intent.Status);
        builder.HasIndex(intent => intent.RequestedAtUtc);

        builder.HasOne<Farm>()
            .WithMany()
            .HasForeignKey(intent => intent.FarmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Seller>()
            .WithMany()
            .HasForeignKey(intent => intent.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(intent => intent.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SellerProduct>()
            .WithMany()
            .HasForeignKey(intent => intent.SellerProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
