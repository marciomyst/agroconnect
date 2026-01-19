using Agronomia.Domain.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class SellerMembershipConfiguration : IEntityTypeConfiguration<SellerMembership>
{
    public void Configure(EntityTypeBuilder<SellerMembership> builder)
    {
        builder.ToTable("seller_memberships");

        builder.HasKey(membership => membership.Id);

        builder.Property(membership => membership.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(membership => membership.SellerId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(membership => membership.UserId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(membership => membership.Role)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();
    }
}
