using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;

public sealed record RegisterSellerWithOwnerCommand(
    Guid UserId,
    string TaxId,
    string CorporateName
) : ICommand<RegisterSellerWithOwnerResult>;
