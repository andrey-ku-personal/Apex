using FluentValidation;
using FluentValidation.Results;

namespace Apex.Invest.Services;

public class ValidationRunner(IServiceProvider sp) : IValidationRunner
{
    public async Task ValidateAsync(object? model, CancellationToken ct = default)
    {
        if (model is null) return;

        var modelType = model.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(modelType);

        var validatorObj = sp.GetService(validatorType) ??
            throw new InvalidOperationException($"Validator for {modelType.Name} not registered");

        var result = await ((dynamic)validatorObj).ValidateAsync((dynamic)model, ct) as ValidationResult;
        if (result is not null && !result.IsValid)
            throw new Apex.Shared.Core.Exceptions.ValidationException(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));
    }
}
