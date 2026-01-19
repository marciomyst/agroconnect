using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Farms.RegisterFarmWithOwner;

public sealed record RegisterFarmWithOwnerCommand(
    Guid UserId,
    string TaxId,
    string Name
) : ICommand<RegisterFarmWithOwnerResult>;
