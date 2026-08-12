using dddnet8.Domain.Shared;


namespace dddnet8.Domain.OperationTypes.Names;


public class Name : ValueObject, IEquatable<Name>
{
    public string _name { get; private set; }
    
    public const int MaxLength = 100;
    
    // Método Create
    public Name(string name)
    {
        if (null == name)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (name.Trim() == string.Empty)
        {
            throw new ArgumentException("Name cannot be empty or whitespace.");
        }

        if (name.Length > MaxLength)
        {
            throw new ArgumentException($"Name cannot be longer than {MaxLength} characters.");
        }

        this._name = name.Trim();
    }
    
    public string Value => _name;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return _name;
    }

    public override string ToString()
    {
        return _name; // Retorna o nome completo
    }

    public bool Equals(Name? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && _name == other._name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Name)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _name);
    }
}