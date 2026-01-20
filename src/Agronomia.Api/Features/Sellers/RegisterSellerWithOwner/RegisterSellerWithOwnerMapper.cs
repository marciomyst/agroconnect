using Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;

namespace Agronomia.Api.Features.Sellers.RegisterSellerWithOwner;

public static class RegisterSellerWithOwnerMapper
{
    public static RegisterSellerWithOwnerCommand ToCommand(this RegisterSellerWithOwnerHttpRequest request, Guid userId)
    {
        return new RegisterSellerWithOwnerCommand(userId, request.TaxId, request.CorporateName);
    }

    public static RegisterSellerWithOwnerHttpResponse FromResult(this RegisterSellerWithOwnerResult result)
    {
        return new RegisterSellerWithOwnerHttpResponse(result.SellerId, result.SellerMembershipId);
    }
}
