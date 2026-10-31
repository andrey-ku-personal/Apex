namespace Apex.Shared.Core.Services;

public interface IValidationRunner
{
    Task ValidateAsync<TModel>(TModel model, CancellationToken cancellationToken = default);
}