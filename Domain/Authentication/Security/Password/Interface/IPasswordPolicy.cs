namespace App.PassswordPolicy;

public interface IPasswordPolicy
{
    bool isSatisfiedBy(string password);
}