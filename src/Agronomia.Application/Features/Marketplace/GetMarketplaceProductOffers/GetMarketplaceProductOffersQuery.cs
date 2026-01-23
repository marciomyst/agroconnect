using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;

public sealed record GetMarketplaceProductOffersQuery(
    Guid FarmId,
    Guid ExecutorUserId,
    Guid ProductId)
    : IQuery<IReadOnlyList<MarketplaceProductOfferItemDto>>;
