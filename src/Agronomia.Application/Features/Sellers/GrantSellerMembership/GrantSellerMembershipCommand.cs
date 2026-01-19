using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.Sellers.GrantSellerMembership;

public sealed record GrantSellerMembershipCommand(
    Guid ExecutorUserId,
    Guid SellerId,
    Guid TargetUserId,
    SellerRole Role
) : ICommand<GrantSellerMembershipResult>;
