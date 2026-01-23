using Agronomia.Application.Abstractions.Catalog;
using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Application.Abstractions.Memberships;

namespace Agronomia.Application.Features.SellerCatalog.GetSellerCatalog;

public sealed class GetSellerCatalogHandler(
    ISellerCatalogReadRepository readRepository,
    ISellerMembershipRepository sellerMembershipRepository)
    : IQueryHandler<GetSellerCatalogQuery, PagedResult<SellerCatalogItemDto>>
{
    private readonly ISellerCatalogReadRepository _readRepository = readRepository;
    private readonly ISellerMembershipRepository _sellerMembershipRepository = sellerMembershipRepository;

    public async Task<PagedResult<SellerCatalogItemDto>> HandleAsync(GetSellerCatalogQuery query, CancellationToken ct)
    {
        var memberships = await _sellerMembershipRepository.GetBySellerAndUserAsync(
            query.SellerId,
            query.ExecutorUserId,
            ct);

        if (memberships.Count == 0)
        {
            throw new SellerCatalogForbiddenException();
        }

        var criteria = new SellerCatalogCriteria(
            query.SellerId,
            query.Search,
            query.Page,
            query.PageSize);

        return await _readRepository.GetSellerCatalogAsync(criteria, ct);
    }
}
