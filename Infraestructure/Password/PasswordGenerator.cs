using App.PassswordPolicy;
using System.Text;
using App.Password.Generator;

public class PasswordGenerator : IPasswordGenerator
{
    private readonly IPasswordPolicy _policy; 
    private static readonly Random _random = new Random();

    public PasswordGenerator(IPasswordPolicy policy)
    {
        _policy = policy ?? throw new ArgumentNullException(nameof(policy)); 
    }

    public string GeneratePassword()
    {
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string numberChars = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+[]{};:,.<>?";

        char[] passwordArray = new char[10];

        passwordArray[0] = uppercaseChars[_random.Next(uppercaseChars.Length)];
        passwordArray[1] = lowercaseChars[_random.Next(lowercaseChars.Length)];
        passwordArray[2] = numberChars[_random.Next(numberChars.Length)];
        passwordArray[3] = specialChars[_random.Next(specialChars.Length)];

        // Preencher o restante da senha com caracteres aleatórios
        for (int i = 4; i < passwordArray.Length; i++)
        {
            string allChars = uppercaseChars + lowercaseChars + numberChars + specialChars;
            passwordArray[i] = allChars[_random.Next(allChars.Length)];
        }

        // Embaralha a senha antes de retornar
        string password = new string(passwordArray.OrderBy(x => _random.Next()).ToArray());

        // Verifica se a senha gerada satisfaz a política
        while (!_policy.isSatisfiedBy(password))
        {
            // Regenera a senha se não satisfaz a política
            password = GeneratePassword();
        }

        return password;
    }
}