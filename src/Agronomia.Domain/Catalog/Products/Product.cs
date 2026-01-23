using Agronomia.Domain.Common;

namespace Agronomia.Domain.Catalog.Products;

public sealed class Product : AggregateRoot
{
    private Product()
    {
    }

    private Product(
        Guid id,
        ProductName name,
        ProductCategory category,
        UnitOfMeasure unitOfMeasure,
        RegistrationNumber? registrationNumber,
        bool isControlledByRecipe,
        bool isActive,
        DateTime createdAtUtc)
        : base(id)
    {
        Name = name;
        Category = category;
        UnitOfMeasure = unitOfMeasure;
        RegistrationNumber = registrationNumber;
        IsControlledByRecipe = isControlledByRecipe;
        IsActive = isActive;
        CreatedAtUtc = createdAtUtc;
    }

    public ProductName Name { get; private set; } = null!;

    public ProductCategory Category { get; private set; }

    public UnitOfMeasure UnitOfMeasure { get; private set; }

    public RegistrationNumber? RegistrationNumber { get; private set; }

    public bool IsControlledByRecipe { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public static Product Create(
        ProductName name,
        ProductCategory category,
        UnitOfMeasure unitOfMeasure,
        RegistrationNumber? registrationNumber,
        bool isControlledByRecipe,
        DateTime? nowUtc = null)
    {
        Guard.AgainstNull(name, nameof(name));

        if (!Enum.IsDefined(typeof(ProductCategory), category))
        {
            throw new ArgumentOutOfRangeException(nameof(category), "Category is invalid.");
        }

        if (!Enum.IsDefined(typeof(UnitOfMeasure), unitOfMeasure))
        {
            throw new ArgumentOutOfRangeException(nameof(unitOfMeasure), "Unit of measure is invalid.");
        }

        var createdAtUtc = NormalizeUtc(nowUtc ?? DateTime.UtcNow);

        return new Product(
            Guid.NewGuid(),
            name,
            category,
            unitOfMeasure,
            registrationNumber,
            isControlledByRecipe,
            isActive: true,
            createdAtUtc);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
