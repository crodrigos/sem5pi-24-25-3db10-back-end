using System.Text.RegularExpressions;
using dddnet8.Domain.Shared;

namespace dddnet8.Domain.SystemUsers
{
    public class EmailAddress : ValueObject
    {
        public string LocalPart { get; private set; } 
        public string Domain { get; private set; }     
        
        private static readonly Regex EmailRegex = 
            new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

        public EmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address cannot be null or empty.");

            email = email.Trim();

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email address format.");

            var parts = email.Split('@');
            LocalPart = parts[0];
            Domain = parts[1];
        }
        
        protected EmailAddress(){}
        
        
        public static EmailAddress Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email address cannot be null or empty.", nameof(email));
            }

            if (!EmailRegex.IsMatch(email))
            {
                throw new ArgumentException("Invalid email format.", nameof(email));
            }

            return new EmailAddress(email);
        }

        private bool IsValidEmail(string email) => 
            EmailRegex.IsMatch(email);

        public string GetFullEmail() => 
            $"{LocalPart}@{Domain}";

        public override string ToString() => 
            GetFullEmail();

        public static EmailAddress Parse(string email) => 
            new EmailAddress(email);

        public bool IsFromDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                throw new ArgumentException("Domain cannot be null or empty.");

            return string.Equals(Domain, domain, StringComparison.OrdinalIgnoreCase);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LocalPart;
            yield return Domain;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EmailAddress other)
            {
                return false;
            }

            return LocalPart == other.LocalPart && Domain == other.Domain;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LocalPart, Domain);
        }
    }
}
