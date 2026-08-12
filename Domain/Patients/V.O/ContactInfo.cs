using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Shared;
using dddnet8.Domain.SystemUsers;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Domain.Patients.V.O
{
    [Owned]
    public class ContactInfo : ValueObject
    {
        public PhoneNumber PhoneNumber { get; private set; }
        public EmailAddress EmailAddress { get; private set; }

        public ContactInfo(PhoneNumber phoneNumber, EmailAddress emailAddress)
        {
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
        }
        
        public ContactInfo(){}
        
        public static ContactInfo Create(ContactInfoDto contactInfoDto)
        {
            PhoneNumber? phoneNumber = null;
            EmailAddress? emailAddress = null;

            if (!string.IsNullOrWhiteSpace(contactInfoDto.PhoneNumber))
            {
                phoneNumber = PhoneNumber.Create(contactInfoDto.PhoneNumber);
            }
            
            if (!string.IsNullOrWhiteSpace(contactInfoDto.EmailAddress))
            {
                emailAddress = EmailAddress.Create(contactInfoDto.EmailAddress);
            }

            return new ContactInfo(phoneNumber, emailAddress);
        }

        // Sobrescrevendo Equals e GetHashCode para comparação de value objects
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (ContactInfo)obj;
            return PhoneNumber.Equals(other.PhoneNumber) && EmailAddress.Equals(other.EmailAddress);
        }
        

        public override int GetHashCode()
        {
            return HashCode.Combine(PhoneNumber, EmailAddress);
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            // Retorna os valores que compõem o objeto
            yield return PhoneNumber;
            yield return EmailAddress;
        }
        public override string ToString()
        {
            return $"{EmailAddress};{PhoneNumber}"; // Formato ajustado para 'Email;Phone'
        }

        public static ContactInfo FromString(string contactInfoString)
        {
            var parts = contactInfoString.Split(';');
            if (parts.Length != 2)
                throw new FormatException("Contact information must be in the format 'Email;Phone'.");

            
            var email = new EmailAddress(parts[0].Trim()); // Correto
            var phone = new PhoneNumber(parts[1].Trim()); // Correto

            return new ContactInfo(phone, email);
        }

        private void UpdatePhoneNumber(string newPhoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                PhoneNumber = PhoneNumber.Create(newPhoneNumber);
            }
        }

        private void UpdateEmailAddress(string newEmailAddress)
        {
            if (!string.IsNullOrWhiteSpace(newEmailAddress))
            {
                EmailAddress = EmailAddress.Create(newEmailAddress);
            }
        }

        public void UpdateContactInformation(ContactInfoDto newContactInfo)
        {
            if(newContactInfo.PhoneNumber != null){UpdatePhoneNumber(newContactInfo.PhoneNumber);}
            if(newContactInfo.EmailAddress != null){UpdateEmailAddress(newContactInfo.EmailAddress);}
        }
    }
}