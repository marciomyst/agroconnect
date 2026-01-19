using Agronomia.Domain.Organizations;
using Agronomia.Domain.Organizations.Events;
using Xunit;

namespace Agronomia.Domain.Tests.Organizations;

public sealed class SellerTests
{
    [Fact]
    public void Register_ValidSeller_RaisesSellerRegistered()
    {
        var seller = Seller.Register("123456789", "Acme Trading");

        var domainEvent = Assert.Single(seller.DomainEvents);
        var registered = Assert.IsType<SellerRegistered>(domainEvent);

        Assert.Equal(seller.Id, registered.SellerId);
        Assert.Equal(seller.TaxId, registered.TaxId);
        Assert.Equal(seller.CorporateName, registered.CorporateName);
        Assert.Equal(DateTimeKind.Utc, registered.OccurredAtUtc.Kind);
    }

    [Fact]
    public void Register_InvalidTaxId_Throws()
    {
        Assert.Throws<ArgumentException>(() => Seller.Register(" ", "Acme Trading"));
    }

    [Fact]
    public void Register_InvalidCorporateName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Seller.Register("123456789", ""));
    }
}
