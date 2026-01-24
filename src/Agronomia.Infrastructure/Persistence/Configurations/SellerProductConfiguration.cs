using Agronomia.Domain.Catalog.SellerProducts;
using Agronomia.Domain.Organizations;
using Agronomia.Domain.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class SellerProductConfiguration : IEntityTypeConfiguration<SellerProduct>
{
    public void Configure(EntityTypeBuilder<SellerProduct> builder)
    {
        builder.ToTable("seller_products");

        builder.HasKey(sellerProduct => sellerProduct.Id);

        builder.Property(sellerProduct => sellerProduct.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(sellerProduct => sellerProduct.SellerId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(sellerProduct => sellerProduct.ProductId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.OwnsOne(sellerProduct => sellerProduct.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("Price")
                .HasColumnType("numeric")
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasConversion<string>()
                .HasMaxLength(8)
                .IsRequired();
        });

        builder.Property(sellerProduct => sellerProduct.IsAvailable)
            .IsRequired();

        builder.Property(sellerProduct => sellerProduct.CreatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(sellerProduct => sellerProduct.UpdatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasIndex(sellerProduct => new { sellerProduct.SellerId, sellerProduct.ProductId })
            .IsUnique();

        builder.HasIndex(sellerProduct => sellerProduct.SellerId);
        builder.HasIndex(sellerProduct => sellerProduct.ProductId);
        builder.HasIndex(sellerProduct => sellerProduct.IsAvailable);

        builder.HasOne<Seller>()
            .WithMany()
            .HasForeignKey(sellerProduct => sellerProduct.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(sellerProduct => sellerProduct.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
