namespace Apex.Shared.Core.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException() : base() { }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception innerException) : base(message, innerException) { }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base(string.Join("; ", errors.SelectMany(x => x.Value)))
    {
        Errors = errors;
    }
}