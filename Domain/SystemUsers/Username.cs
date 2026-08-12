using dddnet8.Domain.Shared;

namespace dddnet8.Domain.SystemUsers;

public class Username : ValueObject
{
    public string Value { get; private set; }
    
    // Construtor privado para forçar o uso do método Create
    public Username(string value)
    {
        Value = value;
    }

    // Método de fábrica para criar um Username
    public static Username Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Username cannot be null or empty.", nameof(value));

        return new Username(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        return obj is Username username && Value == username.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
