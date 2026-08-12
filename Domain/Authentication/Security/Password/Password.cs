using System.Security.Cryptography;
using System.Text;
using dddnet8.Domain.Shared;
using dddnet8.Infraestructure.Password;

namespace dddnet8.Domain.SystemUsers;

public class Password : ValueObject
{
    private readonly string _hashedPassword;

    private Password(string rawPassword)
    {
        var policy = new PasswordPolicy();
        policy.isSatisfiedBy(rawPassword);

        _hashedPassword = HashPassword(rawPassword);
    }
    
    public static Password Create(string rawPassword)
    {
        return new Password(rawPassword);
    }
    
    public string GetHashedPassword()
    {
        return _hashedPassword;
    }
    
    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower(); // Convert to hex string
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return _hashedPassword;
    }
}