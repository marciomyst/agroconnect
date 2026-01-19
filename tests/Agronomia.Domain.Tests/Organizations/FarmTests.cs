using Agronomia.Domain.Organizations;
using Agronomia.Domain.Organizations.Events;
using Xunit;

namespace Agronomia.Domain.Tests.Organizations;

public sealed class FarmTests
{
    [Fact]
    public void Register_ValidFarm_RaisesFarmRegistered()
    {
        var farm = Farm.Register("123456789", "Green Valley");

        var domainEvent = Assert.Single(farm.DomainEvents);
        var registered = Assert.IsType<FarmRegistered>(domainEvent);

        Assert.Equal(farm.Id, registered.FarmId);
        Assert.Equal(farm.TaxId, registered.TaxId);
        Assert.Equal(DateTimeKind.Utc, registered.OccurredAtUtc.Kind);
    }

    [Fact]
    public void Register_InvalidTaxId_Throws()
    {
        Assert.Throws<ArgumentException>(() => Farm.Register(" ", "Green Valley"));
    }

    [Fact]
    public void Register_InvalidName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Farm.Register("123456789", ""));
    }
}
