using System.Net;
using dddnet8.Domain.BackOfficeEmail;
using dddnet8.Domain.SystemUsers;
using YourNamespace.Domain;

namespace App.Email.Generator;

public class BackofficeEmailGenerator: IBackOfficeEmailGenerator
{
    //TODO: VERY BAD PRACTICE
    private const string Dns = "TrelloHospital.com"; // Define a constante DNS
    
    public EmailAddress GenerateStaffEmail(string staffCode)
    {

        if (String.IsNullOrEmpty(staffCode))
        {
            throw new ArgumentException("Staff code is Null or Empty. Staff code is required");
        }
        
        return new EmailAddress($"{staffCode}@{Dns}");
    }
}