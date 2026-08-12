namespace dddnet8.Infraestructure.Shared;

public class Result<T>
{
    // The result design pattern allows to handle operations that can either succeed or fail without relying on exceptions.
    private readonly T? _value;
    private readonly string? _error; // TODO: for now its only a string for simplicity

    private Result(T? value, string? error)
    {
        _value = value;
        _error = error;
    }

    public T Value => _error == null
        ? _value!
        : throw new InvalidOperationException("Cannot access Value when Result is an error.");

    public string? Error => _error;

    public bool IsSuccess => _error == null;
    public bool IsFailure => !IsSuccess;
    
    // Factory methods for Ok and Err
    public static Result<T> Ok(T value) => new(value, null);
    public static Result<T> Err(string error) => new(default, error);
    
    // Implicit conversion for less boilerplate
    public static implicit operator Result<T>(T value) => Ok(value);
    public static implicit operator Result<T>(string error) => Err(error);
}