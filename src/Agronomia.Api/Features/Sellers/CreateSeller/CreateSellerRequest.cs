using Agronomia.Application.Features.Sellers.CreateSeller;
using System.ComponentModel.DataAnnotations;

namespace Agronomia.Api.Features.Sellers.CreateSeller;

/// <summary>
/// Request payload to create a new seller.
/// </summary>
public sealed class CreateSellerRequest
{
    [Required]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    public string StateRegistration { get; set; } = string.Empty;

    [Required]
    public string LegalName { get; set; } = string.Empty;

    [Required]
    public string TradeName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Required]
    public string ContactPhone { get; set; } = string.Empty;

    [Required]
    public string ResponsibleName { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    public string? Complement { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;

    public CreateSellerCommand ToCommand() =>
        new(
            Cnpj,
            StateRegistration,
            LegalName,
            TradeName,
            ContactEmail,
            ContactPhone,
            ResponsibleName,
            ZipCode,
            Street,
            Number,
            City,
            State,
            Complement,
            Password);
}