namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed class SellerMembershipForbiddenException()
    : UnauthorizedAccessException("Only a Seller Owner can grant memberships.");
