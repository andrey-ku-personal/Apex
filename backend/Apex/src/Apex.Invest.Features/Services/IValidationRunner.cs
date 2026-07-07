namespace Apex.Invest.Services;

public interface IValidationRunner
{
    Task ValidateAsync(object? model, CancellationToken ct = default);
}
