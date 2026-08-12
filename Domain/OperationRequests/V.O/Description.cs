using dddnet8.Domain.Shared;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dddnet8.Domain.OperationRequests;

public class Description : ValueObject
{
    private const int MaxLength = 512; // Business Rule

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }
    
    // Construtor sem parâmetros para o Entity Framework
    public Description() {}
    
    
    

    public static Result<Description> Create(string value)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
        {
            return Result<Description>.Err("Description cannot be null or empty");
        }

        if (value.Length > MaxLength)
        {
            return Result<Description>.Err($"Description cannot exceed {MaxLength} characters.");
        }

        return new Description(value);
    }
    
    public override string ToString()
    {
        return Value;
    }
    
    // Same as factory method (for convinience)
    public static Result<Description> FromString(string description)
    {
        return Create(description);
    }
    
    // Operador de conversão explícita de Description para string
    public static explicit operator string(Description description)
    {
        return description?.Value ?? string.Empty;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}