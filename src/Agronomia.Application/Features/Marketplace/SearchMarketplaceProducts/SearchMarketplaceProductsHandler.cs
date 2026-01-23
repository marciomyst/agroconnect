using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.Marketplace.SearchMarketplaceProducts;

public sealed class SearchMarketplaceProductsHandler(
    IMarketplaceReadRepository readRepository,
    IFarmMembershipRepository farmMembershipRepository)
    : IQueryHandler<SearchMarketplaceProductsQuery, PagedResult<MarketplaceProductListItemDto>>
{
    private readonly IMarketplaceReadRepository _readRepository = readRepository;
    private readonly IFarmMembershipRepository _farmMembershipRepository = farmMembershipRepository;

    public async Task<PagedResult<MarketplaceProductListItemDto>> HandleAsync(
        SearchMarketplaceProductsQuery query,
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

        var criteria = new MarketplaceSearchCriteria(
            query.Search,
            query.Category,
            query.Page,
            query.PageSize);

        return await _readRepository.SearchProductsAsync(criteria, ct);
    }
}
