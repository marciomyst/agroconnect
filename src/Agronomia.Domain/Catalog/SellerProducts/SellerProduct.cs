using Agronomia.Domain.Catalog.ValueObjects;
using Agronomia.Domain.Common;

namespace Agronomia.Domain.Catalog.SellerProducts;

public sealed class SellerProduct : AggregateRoot
{
    private SellerProduct()
    {
    }

    private SellerProduct(
        Guid id,
        Guid sellerId,
        Guid productId,
        Money price,
        bool isAvailable,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
        : base(id)
    {
        SellerId = sellerId;
        ProductId = productId;
        Price = price;
        IsAvailable = isAvailable;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public Guid SellerId { get; private set; }

    public Guid ProductId { get; private set; }

    public Money Price { get; private set; } = null!;

    public bool IsAvailable { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static SellerProduct Create(
        Guid sellerId,
        Guid productId,
        Money price,
        bool isAvailable,
        DateTime? nowUtc = null)
    {
        if (sellerId == Guid.Empty)
        {
            throw new ArgumentException("SellerId is required.", nameof(sellerId));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("ProductId is required.", nameof(productId));
        }

        Guard.AgainstNull(price, nameof(price));

        var timestamp = NormalizeUtc(nowUtc ?? DateTime.UtcNow);

        return new SellerProduct(
            Guid.NewGuid(),
            sellerId,
            productId,
            price,
            isAvailable,
            timestamp,
            timestamp);
    }

    public void Update(Money price, bool isAvailable, DateTime? nowUtc = null)
    {
        Guard.AgainstNull(price, nameof(price));

        Price = price;
        IsAvailable = isAvailable;
        UpdatedAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
