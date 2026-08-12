using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Patients.V.O;

public class SpecializationCode : ValueObject, IEquatable<SpecializationCode>
{
    private readonly string _code; // Variável privada

    public string Code => _code; // Propriedade pública somente leitura

    private SpecializationCode(string value)
    {
        _code = value;
    }
    
    

    // Método Create
    public static SpecializationCode Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot have only whitespace.");
        }

        return new SpecializationCode(value.Trim()); // Retorna uma nova instância de Name após validação
    }

    protected SpecializationCode(){}

    public override bool Equals(object? obj)
    {
        return obj is SpecializationCode other && Equals(other);
    }

    public bool Equals(SpecializationCode? other)
    {
        if (other is null) return false;
        return string.Equals(_code, other._code, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return _code.ToLowerInvariant().GetHashCode();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return _code;
    }

    public override string ToString()
    {
        return _code; // Retorna o nome completo
    }
    
    public static explicit operator string(SpecializationCode name) => name._code;

    
    public static explicit operator SpecializationCode(string value) => Create(value);

    public bool Contains(SpecializationCode other)
    {
        if (other is null || string.IsNullOrEmpty(other._code))
        {
            return false;
        }

        return _code.Contains(other._code, StringComparison.OrdinalIgnoreCase);
    }

}