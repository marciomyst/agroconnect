using Agronomia.Domain.Common;

namespace Agronomia.Domain.Orders.PurchaseIntents;

public sealed class PurchaseIntent : AggregateRoot
{
    private PurchaseIntent()
    {
    }

    private PurchaseIntent(
        Guid id,
        Guid farmId,
        Guid sellerId,
        Guid productId,
        Guid sellerProductId,
        Quantity quantity,
        string? notes,
        PurchaseIntentStatus status,
        DateTime requestedAtUtc,
        DateTime updatedAtUtc)
        : base(id)
    {
        FarmId = farmId;
        SellerId = sellerId;
        ProductId = productId;
        SellerProductId = sellerProductId;
        Quantity = quantity;
        Notes = notes;
        Status = status;
        RequestedAtUtc = requestedAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public Guid FarmId { get; private set; }

    public Guid SellerId { get; private set; }

    public Guid ProductId { get; private set; }

    public Guid SellerProductId { get; private set; }

    public Quantity Quantity { get; private set; } = null!;

    public string? Notes { get; private set; }

    public PurchaseIntentStatus Status { get; private set; }

    public DateTime RequestedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public static PurchaseIntent Create(
        Guid farmId,
        Guid sellerId,
        Guid productId,
        Guid sellerProductId,
        Quantity quantity,
        string? notes,
        DateTime? nowUtc = null)
    {
        if (farmId == Guid.Empty)
        {
            throw new ArgumentException("FarmId is required.", nameof(farmId));
        }

        if (sellerId == Guid.Empty)
        {
            throw new ArgumentException("SellerId is required.", nameof(sellerId));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("ProductId is required.", nameof(productId));
        }

        if (sellerProductId == Guid.Empty)
        {
            throw new ArgumentException("SellerProductId is required.", nameof(sellerProductId));
        }

        Guard.AgainstNull(quantity, nameof(quantity));

        var timestamp = NormalizeUtc(nowUtc ?? DateTime.UtcNow);
        var normalizedNotes = NormalizeNotes(notes);

        return new PurchaseIntent(
            Guid.NewGuid(),
            farmId,
            sellerId,
            productId,
            sellerProductId,
            quantity,
            normalizedNotes,
            PurchaseIntentStatus.Pending,
            timestamp,
            timestamp);
    }

    public void UpdateStatus(PurchaseIntentStatus status, DateTime? nowUtc = null)
    {
        if (!Enum.IsDefined(typeof(PurchaseIntentStatus), status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), "Status is invalid.");
        }

        if (Status == status)
        {
            return;
        }

        if (Status != PurchaseIntentStatus.Pending)
        {
            throw new InvalidOperationException("Only pending intents can change status.");
        }

        if (status is not (PurchaseIntentStatus.Accepted or PurchaseIntentStatus.Rejected or PurchaseIntentStatus.Expired))
        {
            throw new InvalidOperationException("Status transition is not allowed.");
        }

        Status = status;
        UpdatedAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);
    }

    private static string? NormalizeNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            return null;
        }

        return notes.Trim();
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
