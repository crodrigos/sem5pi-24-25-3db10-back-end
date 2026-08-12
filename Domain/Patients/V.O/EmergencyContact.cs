using App.Onion.Application.Dtos;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Shared;
using Microsoft.EntityFrameworkCore;


namespace dddnet8.Domain.Patients.VO.Name;

[Owned] 
public class EmergencyContact : ValueObject
{
    public V.O.Name EmergencyContactName { get; private set; }
    public PhoneNumber EmergencyContactPhoneNumber { get; private set; }

    public EmergencyContact(V.O.Name emergencyContactName, PhoneNumber emergencyContactPhoneNumber)
    {
        EmergencyContactName = emergencyContactName ?? throw new ArgumentNullException(nameof(emergencyContactName), "Name cannot be null in Emergency Contact.");
        EmergencyContactPhoneNumber = emergencyContactPhoneNumber ?? throw new ArgumentNullException(nameof(emergencyContactPhoneNumber), "Phone number cannot be null in Emergency contact.");
    }
    
    protected EmergencyContact(){}
    
    public static EmergencyContact Create(string contactName, string contactPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(contactName))
            throw new ArgumentException("Contact name cannot be null or empty.", nameof(contactName));

        if (string.IsNullOrWhiteSpace(contactPhoneNumber))
            throw new ArgumentException("Contact phone number cannot be null or empty.", nameof(contactPhoneNumber));

        var name =  V.O.Name.Create(contactName);
        var phoneNumber = new PhoneNumber(contactPhoneNumber);

        return new EmergencyContact(name, phoneNumber);
    }

    public static EmergencyContact FromString(string contactInfoString)
    {
        if (string.IsNullOrWhiteSpace(contactInfoString))
            throw new ArgumentNullException(nameof(contactInfoString), "Contact information cannot be null or empty.");

        var parts = contactInfoString.Split(';');
        if (parts.Length != 2)
            throw new ArgumentException("Emergency contact must be in the format 'Name;PhoneNumber'.");

        return Create(parts[0].Trim(), parts[1].Trim());
    }

    
    public override string ToString()
    {
        return $"{EmergencyContactName};{EmergencyContactPhoneNumber}"; // Formato ajustado para 'Name;Phone'
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return EmergencyContactName;
        yield return EmergencyContactPhoneNumber;
    }

    private void UpdateName(string newName)
    {
        if (!string.IsNullOrWhiteSpace(newName))
        {
            EmergencyContactName = V.O.Name.Create(newName);
        }
    }

    private void UpdatePhoneNumber(string newPhoneNumber)
    {
        if (!string.IsNullOrWhiteSpace(newPhoneNumber))
        {
            EmergencyContactPhoneNumber = PhoneNumber.Create(newPhoneNumber);
        }
    }

    public void UpdateEmergencyContact(EmergencyContactDto newEmergencyContact)
    {
        if (newEmergencyContact.EmergencyContactName != null) {UpdateName(newEmergencyContact.EmergencyContactName);} 
        if (newEmergencyContact.EmergencyContactPhoneNumber != null){UpdatePhoneNumber(newEmergencyContact.EmergencyContactPhoneNumber);}
    }
}
