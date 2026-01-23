namespace Agronomia.Application.Features.Marketplace;

public sealed class MarketplaceForbiddenException()
    : Exception("User does not have permission to access the marketplace.")
{
}
