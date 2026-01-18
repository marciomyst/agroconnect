namespace Agronomia.Application.Features.Sellers.CreateSeller;

/// <summary>
/// Command to register a new seller (revenda).
/// </summary>
public sealed record CreateSellerCommand(
    string Cnpj,
    string StateRegistration,
    string LegalName,
    string TradeName,
    string ContactEmail,
    string ContactPhone,
    string ResponsibleName,
    string ZipCode,
    string Street,
    string Number,
    string City,
    string State,
    string? Complement,
    string Password);
