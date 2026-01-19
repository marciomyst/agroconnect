namespace Agronomia.Application.Features.Farms.RegisterFarmWithOwner;

public sealed class FarmTaxIdAlreadyExistsException(string taxId)
    : InvalidOperationException($"Farm with TaxId '{taxId}' already exists.");
