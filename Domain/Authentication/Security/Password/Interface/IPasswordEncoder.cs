namespace App.Passsword.Encoder;

public interface IPasswordEncoder
{
    string Encode(string password);

    bool Verify(string password, string hashedPassword);
}