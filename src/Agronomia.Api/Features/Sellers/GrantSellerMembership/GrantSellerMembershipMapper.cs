using Agronomia.Application.Features.Sellers.GrantSellerMembership;

namespace Agronomia.Api.Features.Sellers.GrantSellerMembership;

public static class GrantSellerMembershipMapper
{
    public static GrantSellerMembershipCommand ToCommand(
        this GrantSellerMembershipHttpRequest request,
        Guid executorUserId,
        Guid sellerId)
    {
        return new GrantSellerMembershipCommand(
            executorUserId,
            sellerId,
            request.UserId,
            request.Role);
    }

    public static GrantSellerMembershipHttpResponse FromResult(this GrantSellerMembershipResult result)
    {
        return new GrantSellerMembershipHttpResponse(result.SellerMembershipId);
    }
}
