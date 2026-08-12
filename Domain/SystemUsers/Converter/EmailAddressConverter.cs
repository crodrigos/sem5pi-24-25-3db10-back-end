using dddnet8.Domain.SystemUsers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YourNamespace.Domain;

namespace App.Domain.SystemUser
{
    public class EmailAddressConverter : ValueConverter<EmailAddress, string>
    {
        public EmailAddressConverter()
            : base(
                emailAddress => emailAddress.ToString(), 
                str => EmailAddress.Parse(str)
            )
        {
        }
    }
}