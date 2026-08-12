namespace dddnet8.Domain.Shared;

/*
public class PasswordPolicy
{
    public int MinLength { get; set; }
    
    public bool IsSatisfiedBy(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty");
        }

        if (password.Length < MinLength)
        {
            throw new ArgumentException($"Password must be at least {MinLength} characters long.");
        }

        if (!password.Any(char.IsDigit))
        {
            throw new ArgumentException("Password must contain at least one digit.");
        }

        if (!password.Any(char.IsUpper))
        {
            throw new ArgumentException("Password must contain at least one uppercase letter.");
        }

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            throw new ArgumentException("Password must contain at least one special character.");
        }

        return true;
    }
}
*/
