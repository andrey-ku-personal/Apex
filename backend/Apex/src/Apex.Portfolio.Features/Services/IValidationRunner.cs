namespace Apex.Portfolio.Services;

public interface IValidationRunner
{
    Task ValidateAsync(object? model, CancellationToken ct = default);
}
