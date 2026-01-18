using Agronomia.Domain.Aggregates.Users;
using Agronomia.Domain.SeedWork;

namespace Agronomia.Domain.Aggregates.Sellers;

/// <summary>
/// Represents a seller (revendedor) that can list products on the marketplace.
/// </summary>
public sealed class Seller : Entity, IAggregateRoot
{
    private Seller()
    {
        // Required by EF Core
    }

    /// <summary>
    /// Creates a new seller aggregate.
    /// </summary>
    /// <param name="legalName">Razão social.</param>
    /// <param name="tradeName">Nome fantasia exibido no marketplace.</param>
    /// <param name="document">Documento fiscal (CNPJ).</param>
    /// <param name="stateRegistration">Inscrição estadual.</param>
    /// <param name="contactEmail">E-mail principal de contato.</param>
    /// <param name="contactPhone">Telefone principal de contato.</param>
    /// <param name="responsibleName">Responsável comercial/contato principal.</param>
    /// <param name="zipCode">CEP.</param>
    /// <param name="street">Logradouro.</param>
    /// <param name="number">Número.</param>
    /// <param name="city">Cidade.</param>
    /// <param name="state">UF.</param>
    /// <param name="complement">Complemento (opcional).</param>
    /// <param name="logoUrl">URL do logotipo exibido nas vitrines.</param>
    /// <param name="id">Identificador opcional. Se não informado, um Guid é gerado.</param>
    /// <param name="createdAt">Data de criação opcional. Caso omitida, usa UTC agora.</param>
    public Seller(
        string legalName,
        string tradeName,
        string document,
        string stateRegistration,
        string contactEmail,
        string contactPhone,
        string responsibleName,
        string zipCode,
        string street,
        string number,
        string city,
        string state,
        string? complement = null,
        string? logoUrl = null,
        string? id = null,
        DateTimeOffset? createdAt = null)
    {
        Id = id ?? Guid.NewGuid().ToString();
        LegalName = legalName;
        TradeName = tradeName;
        Document = document;
        StateRegistration = stateRegistration;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        ResponsibleName = responsibleName;
        ZipCode = zipCode;
        Street = street;
        Number = number;
        City = city;
        State = state;
        Complement = complement;
        LogoUrl = logoUrl;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the legal name (razão social).
    /// </summary>
    public string LegalName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the trade name shown publicly.
    /// </summary>
    public string TradeName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the fiscal document (CNPJ).
    /// </summary>
    public string Document { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the state registration (IE).
    /// </summary>
    public string StateRegistration { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the main contact email.
    /// </summary>
    public string ContactEmail { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the main contact phone.
    /// </summary>
    public string ContactPhone { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the logo URL used in listings.
    /// </summary>
    public string? LogoUrl { get; private set; }

    /// <summary>
    /// Gets the responsible person's name.
    /// </summary>
    public string ResponsibleName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the zip code.
    /// </summary>
    public string ZipCode { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the street / logradouro.
    /// </summary>
    public string Street { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the address number.
    /// </summary>
    public string Number { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the city.
    /// </summary>
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the state (UF).
    /// </summary>
    public string State { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the optional complement.
    /// </summary>
    public string? Complement { get; private set; }

    /// <summary>
    /// Gets when the seller was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the users that can manage this seller account.
    /// </summary>
    public ICollection<User> Managers { get; private set; } = new List<User>();

    /// <summary>
    /// Updates public-facing information.
    /// </summary>
    public void UpdateProfile(string tradeName, string? logoUrl)
    {
        TradeName = tradeName;
        LogoUrl = logoUrl;
    }

    /// <summary>
    /// Updates contact information.
    /// </summary>
    public void UpdateContacts(string contactEmail, string contactPhone)
    {
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
    }

    /// <summary>
    /// Updates address information.
    /// </summary>
    public void UpdateAddress(string zipCode, string street, string number, string city, string state, string? complement)
    {
        ZipCode = zipCode;
        Street = street;
        Number = number;
        City = city;
        State = state;
        Complement = complement;
    }

    /// <summary>
    /// Grants management access to a user.
    /// </summary>
    public void AssignManager(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (Managers.Any(manager => manager.Id == user.Id))
        {
            return;
        }

        Managers.Add(user);

        if (user.ManagedSellers.Any(seller => seller.Id == Id))
        {
            return;
        }

        user.ManagedSellers.Add(this);
    }
}
