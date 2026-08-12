using App.PassswordPolicy;
using dddnet8.Domain.SystemUsers;

namespace dddnet8.Infraestructure.Password;

public class PasswordPolicy : IPasswordPolicy {

    private const int MinLength = 10;

    public bool isSatisfiedBy(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new PasswordException("Password cannot be null or empty");
        }

        if (password.Length < MinLength)
        {
            throw new PasswordException($"Password must be at least {MinLength} characters long.");
        }

        if (!password.Any(char.IsDigit))
        {
            throw new PasswordException("Password must contain at least one digit.");
        }

        if (!password.Any(char.IsUpper))
        {
            throw new PasswordException("Password must contain at least one uppercase letter.");
        }

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            throw new PasswordException("Password must contain at least one special character.");
        }

        return true;
    }
}