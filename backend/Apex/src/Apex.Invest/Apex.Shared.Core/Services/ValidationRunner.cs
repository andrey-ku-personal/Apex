using Apex.Shared.Core.Exceptions;
using FluentValidation;

namespace Apex.Shared.Core.Services;

public sealed class ValidationRunner(IServiceProvider serviceProvider) : IValidationRunner
{
    public async Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken = default)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(typeof(TModel));

        var validator = serviceProvider.GetService(validatorType)
            ?? throw new InvalidOperationException($"Validator for {typeof(TModel).Name} is not registered");

        var result = await ((IValidator<TModel>)validator).ValidateAsync(model, cancellationToken);
        if (!result.IsValid)
            throw new Exceptions.ValidationException(string.Join("; ", result.Errors.Select(x => x.ErrorMessage)));
    }
}