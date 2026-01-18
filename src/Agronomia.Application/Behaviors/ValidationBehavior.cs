using FluentValidation;

namespace Agronomia.Application.Behaviors;

/// <summary>
/// Executes FluentValidation validators before the inner handler.
/// <para>
/// If validation fails, this behavior throws <see cref="ValidationException"/> to stop execution.
/// </para>
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> HandleAsync(
        TRequest request,
        Func<CancellationToken, Task<TResponse>> next,
        CancellationToken ct)
    {
        if (!_validators.Any())
        {
            return await next(ct);
        }

        var context = new ValidationContext<TRequest>(request);
        var failures = new List<FluentValidation.Results.ValidationFailure>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(context, ct);
            if (!result.IsValid)
            {
                failures.AddRange(result.Errors);
            }
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }

        return await next(ct);
    }
}
