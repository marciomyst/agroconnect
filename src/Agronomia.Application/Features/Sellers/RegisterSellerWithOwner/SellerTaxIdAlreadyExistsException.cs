namespace Agronomia.Application.Features.Sellers.RegisterSellerWithOwner;

public sealed class SellerTaxIdAlreadyExistsException(string taxId)
    : InvalidOperationException($"Seller with TaxId '{taxId}' already exists.");
