using FluentValidation;

namespace Agronomia.Application.Features.Sellers.CreateSeller;

internal sealed class CreateSellerCommandValidator : AbstractValidator<CreateSellerCommand>
{
    public CreateSellerCommandValidator()
    {
        RuleFor(c => c.Cnpj)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .MaximumLength(32);

        RuleFor(c => c.StateRegistration)
            .NotEmpty().WithMessage("Inscrição estadual é obrigatória.")
            .MaximumLength(32);

        RuleFor(c => c.LegalName)
            .NotEmpty().WithMessage("Razão social é obrigatória.")
            .MaximumLength(200);

        RuleFor(c => c.TradeName)
            .NotEmpty().WithMessage("Nome fantasia é obrigatório.")
            .MaximumLength(200);

        RuleFor(c => c.ContactEmail)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(256);

        RuleFor(c => c.ContactPhone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .MaximumLength(32);

        RuleFor(c => c.ResponsibleName)
            .NotEmpty().WithMessage("Responsável é obrigatório.")
            .MaximumLength(200);

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter ao menos 8 caracteres.");

        RuleFor(c => c.ZipCode)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .MaximumLength(12);

        RuleFor(c => c.Street)
            .NotEmpty().WithMessage("Logradouro é obrigatório.")
            .MaximumLength(250);

        RuleFor(c => c.Number)
            .NotEmpty().WithMessage("Número é obrigatório.")
            .MaximumLength(32);

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(120);

        RuleFor(c => c.State)
            .NotEmpty().WithMessage("UF é obrigatória.")
            .MaximumLength(4);

        RuleFor(c => c.Complement)
            .MaximumLength(120)
            .When(c => !string.IsNullOrWhiteSpace(c.Complement));
    }
}
