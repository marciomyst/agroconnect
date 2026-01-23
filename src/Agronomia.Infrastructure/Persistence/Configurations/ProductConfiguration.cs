using Agronomia.Domain.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(product => product.Name)
            .HasConversion(
                name => name.Value,
                value => ProductName.Create(value))
            .HasColumnName("Name")
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

        builder.Property(product => product.Category)
            .HasConversion<string>()
            .HasColumnName("Category")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(product => product.UnitOfMeasure)
            .HasConversion<string>()
            .HasColumnName("UnitOfMeasure")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(product => product.RegistrationNumber)
            .HasConversion(
                registration => registration == null ? null : registration.Value,
                value => value == null ? null : RegistrationNumber.Create(value))
            .HasColumnName("RegistrationNumber")
            .HasMaxLength(RegistrationNumber.MaxLength)
            .IsRequired(false);

        builder.Property(product => product.IsControlledByRecipe)
            .IsRequired();

        builder.Property(product => product.IsActive)
            .IsRequired();

        builder.Property(product => product.CreatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasIndex(product => product.Name);
        builder.HasIndex(product => product.Category);

        builder.HasIndex(product => new { product.Name, product.RegistrationNumber })
            .IsUnique()
            .HasFilter("\"RegistrationNumber\" IS NOT NULL");
    }
}
