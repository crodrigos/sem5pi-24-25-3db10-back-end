using dddnet8.Domain.SystemUsers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YourNamespace.Domain;

namespace App.Domain.SystemUser;

public class UserNameConverter : ValueConverter<Username, string>
{
    public UserNameConverter()
        : base(
            username => username.Value, // Converte Username para string
            str => new Username(str) // Converte string de volta para Username
        )
    {
    }
}