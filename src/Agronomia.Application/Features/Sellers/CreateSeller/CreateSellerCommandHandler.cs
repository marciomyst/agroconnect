using Agronomia.Crosscutting.Security;
using Agronomia.Domain.Aggregates.Sellers;
using Agronomia.Domain.Aggregates.Users;
using Agronomia.Domain.Aggregates.Users.ValueObjects;

namespace Agronomia.Application.Features.Sellers.CreateSeller;

/// <summary>
/// Handles seller creation commands.
/// </summary>
public sealed class CreateSellerCommandHandler(ISellerRepository sellerRepository, IUserRepository userRepository)
{
    public async Task<CreateSellerResult> Handle(CreateSellerCommand command, CancellationToken cancellationToken)
    {
        var seller = new Seller(
            legalName: command.LegalName,
            tradeName: command.TradeName,
            document: command.Cnpj,
            stateRegistration: command.StateRegistration,
            contactEmail: command.ContactEmail,
            contactPhone: command.ContactPhone,
            responsibleName: command.ResponsibleName,
            zipCode: command.ZipCode,
            street: command.Street,
            number: command.Number,
            city: command.City,
            state: command.State,
            complement: command.Complement);

        var user = new User(
            email: command.ContactEmail,
            password: PasswordHasher.GenerateValidationHash(command.Password),
            name: command.ResponsibleName,
            role: UserRole.Supervisor);

        seller.AssignManager(user);

        sellerRepository.Add(seller);
        userRepository.Add(user);

        await sellerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return new CreateSellerResult(seller.Id);
    }
}
