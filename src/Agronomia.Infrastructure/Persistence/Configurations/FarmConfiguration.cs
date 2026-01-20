using Agronomia.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class FarmConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.ToTable("farms");

        builder.HasKey(farm => farm.Id);

        builder.Property(farm => farm.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(farm => farm.TaxId)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(farm => farm.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(farm => farm.TaxId)
            .IsUnique();
    }
}
