namespace Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;

public sealed record MarketplaceProductOfferItemDto(
    Guid SellerProductId,
    Guid SellerId,
    string SellerName,
    decimal Price,
    string Currency,
    bool IsAvailable);
