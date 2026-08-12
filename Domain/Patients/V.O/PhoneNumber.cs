using dddnet8.Domain.Shared;


namespace App.Onion.Domain.V.O.Patient;

using System.Text.RegularExpressions;

public class PhoneNumber : ValueObject
{
    public string Number { get; set; }
    
    private static readonly string[] AllowedCountryCodes = { "351", "00351", "9" };

    public PhoneNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Phone number cannot be null or empty.");

        if (!IsValidPortugueseMobileNumber(number))
            throw new ArgumentException("Phone number must start with a valid Portuguese mobile prefix and have 9 digits.");

        if (!ValidatePhoneNumber(number))
            throw new ArgumentException("Phone number must start with a valid Portuguese country code.");

        Number = number;
    }
    
    public static PhoneNumber Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Phone number cannot be null or empty.");

        if (!IsValidPortugueseMobileNumber(number))
            throw new ArgumentException("Phone number must start with a valid Portuguese mobile prefix and have 9 digits.");

        if (!ValidatePhoneNumber(number))
            throw new ArgumentException("Phone number must start with a valid Portuguese country code.");

        return new PhoneNumber(number);
    }

    private static bool IsValidPortugueseMobileNumber(string number)
    {
        // Remove todos os caracteres não numéricos
        var digitsOnly = Regex.Replace(number, @"\D", "");
        
        return digitsOnly.Length == 9 || digitsOnly.Length == 14 || digitsOnly.Length == 12;
    }

    private static bool ValidatePhoneNumber(string number)
    {
        var digitsOnly = Regex.Replace(number, @"\D", "");
        
        return digitsOnly.StartsWith(AllowedCountryCodes[0]) || digitsOnly.StartsWith(AllowedCountryCodes[1]) || digitsOnly.StartsWith(AllowedCountryCodes[2]);
    }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        var other = (PhoneNumber)obj;
        return Number.Equals(other.Number);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Number);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Number;
    }

    public override string ToString()
    {
        return Number;
    }
    
    public static explicit operator string(PhoneNumber phoneNumber) => phoneNumber.Number;

    // Operador explícito para converter string em PhoneNumber
    public static explicit operator PhoneNumber(string value) => Create(value);
    
}