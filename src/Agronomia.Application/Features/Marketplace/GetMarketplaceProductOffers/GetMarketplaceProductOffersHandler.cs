using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.Marketplace.GetMarketplaceProductOffers;

public sealed class GetMarketplaceProductOffersHandler(
    IMarketplaceReadRepository readRepository,
    IFarmMembershipRepository farmMembershipRepository)
    : IQueryHandler<GetMarketplaceProductOffersQuery, IReadOnlyList<MarketplaceProductOfferItemDto>>
{
    private readonly IMarketplaceReadRepository _readRepository = readRepository;
    private readonly IFarmMembershipRepository _farmMembershipRepository = farmMembershipRepository;

    public async Task<IReadOnlyList<MarketplaceProductOfferItemDto>> HandleAsync(
        GetMarketplaceProductOffersQuery query,
        CancellationToken ct)
    {
        var memberships = await _farmMembershipRepository.GetByFarmAndUserAsync(
            query.FarmId,
            query.ExecutorUserId,
            ct);

        if (!memberships.Any(membership => membership.Role is FarmRole.Owner or FarmRole.Buyer))
        {
            throw new MarketplaceForbiddenException();
        }

        return await _readRepository.GetProductOffersAsync(query.ProductId, ct);
    }
}
