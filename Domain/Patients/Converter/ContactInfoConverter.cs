using dddnet8.Domain.Patients.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YourNamespace.Domain;

namespace App.Domain.SystemUser;

public class ContactInfoConverter : ValueConverter<ContactInfo, string>
{
    public ContactInfoConverter() 
        : base(
            v => v.ToString(), // Convert ContactInfo to string
            v => ContactInfo.FromString(v) // Convert string back to ContactInfo
        )
    {
    }
}