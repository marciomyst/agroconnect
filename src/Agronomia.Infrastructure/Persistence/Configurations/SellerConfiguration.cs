using Agronomia.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder.ToTable("sellers");

        builder.HasKey(seller => seller.Id);

        builder.Property(seller => seller.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(seller => seller.TaxId)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(seller => seller.CorporateName)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(seller => seller.TaxId)
            .IsUnique();
    }
}
