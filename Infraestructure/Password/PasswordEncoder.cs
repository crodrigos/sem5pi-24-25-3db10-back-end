namespace App.Passsword.Encoder;

public class PasswordEncoder : IPasswordEncoder
{
    public string Encode(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}