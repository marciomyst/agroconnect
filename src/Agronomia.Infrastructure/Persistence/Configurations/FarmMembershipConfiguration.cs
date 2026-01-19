using Agronomia.Domain.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agronomia.Infrastructure.Persistence.Configurations;

internal sealed class FarmMembershipConfiguration : IEntityTypeConfiguration<FarmMembership>
{
    public void Configure(EntityTypeBuilder<FarmMembership> builder)
    {
        builder.ToTable("farm_memberships");

        builder.HasKey(membership => membership.Id);

        builder.Property(membership => membership.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(membership => membership.FarmId)
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
