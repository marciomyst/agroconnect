using Agronomia.Application.Abstractions.CQRS;
using Agronomia.Domain.Memberships;

namespace Agronomia.Application.Features.Farms.GrantFarmMembership;

public sealed record GrantFarmMembershipCommand(
    Guid ExecutorUserId,
    Guid FarmId,
    Guid TargetUserId,
    FarmRole Role
) : ICommand<GrantFarmMembershipResult>;
