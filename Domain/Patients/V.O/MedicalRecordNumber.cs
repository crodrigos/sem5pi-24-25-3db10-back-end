using dddnet8.Domain.Shared;
public class MedicalRecordNumber : ValueObject
{
    public string Value { get; private set; }

    public MedicalRecordNumber(string value)
    {
        // Valida se o valor é nulo ou vazio
        if (string.IsNullOrEmpty(value) || value.Equals("0"))
            throw new ArgumentException("Medical Record Number must be a positive integer.", nameof(value));
        
        // Verifica se o comprimento do valor é exatamente 12
        if (value.Length != 12)
            throw new ArgumentException("Medical Record Number must be exactly 12 digits long.", nameof(value));

        Value = value;
    }
    
    public static MedicalRecordNumber Create(string conditionName)
    {
        return new MedicalRecordNumber(conditionName);
    }
    
    protected MedicalRecordNumber(){}

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}