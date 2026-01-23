namespace Agronomia.Application.Features.SellerCatalog;

public sealed class SellerCatalogForbiddenException()
    : Exception("User does not have permission to manage the seller catalog.")
{
}
