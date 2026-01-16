using FluentValidation;
using Microsoft.Extensions.Options;

namespace Agronomia.Api.Infrastructure;

public static class OptionsBuilderFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(sp =>
        {
            var validator = sp.GetRequiredService<IValidator<TOptions>>();
            return new FluentValidationOptions<TOptions>(builder.Name, validator);
        });

        return builder;
    }

    private class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly string? _name;
        private readonly IValidator<TOptions> _validator;

        public FluentValidationOptions(string? name, IValidator<TOptions> validator)
        {
            _name = name;
            _validator = validator;
        }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            if (_name != null && _name != name) return ValidateOptionsResult.Skip;

            var result = _validator.Validate(options);

            if (result.IsValid) return ValidateOptionsResult.Success;

            var errors = result.Errors.Select(e => $"Options validation failure for {typeof(TOptions).Name}.{e.PropertyName}: {e.ErrorMessage}");
            return ValidateOptionsResult.Fail(errors);
        }
    }
}