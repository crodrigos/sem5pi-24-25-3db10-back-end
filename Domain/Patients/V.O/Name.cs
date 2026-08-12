using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Patients.V.O;

public class Name : ValueObject, IEquatable<Name>
{
    private readonly string _value; // Variável privada

    public string Value => _value; // Propriedade pública somente leitura

    private Name(string value)
    {
        _value = value;
    }

    // Método Create
    public static Name Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot have only whitespace.");
        }

        return new Name(value.Trim()); // Retorna uma nova instância de Name após validação
    }

    protected Name(){}

    public override bool Equals(object? obj)
    {
        return obj is Name other && Equals(other);
    }

    public bool Equals(Name? other)
    {
        if (other is null) return false;
        return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return _value.ToLowerInvariant().GetHashCode();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return _value;
    }

    public override string ToString()
    {
        return _value; // Retorna o nome completo
    }
    
    public static explicit operator string(Name name) => name._value;

    // Operador explícito para converter string em Name
    public static explicit operator Name(string value) => Create(value);

    public bool Contains(Name other)
    {
        if (other is null || string.IsNullOrEmpty(other._value))
        {
            return false;
        }

        return _value.Contains(other._value, StringComparison.OrdinalIgnoreCase);
    }

}