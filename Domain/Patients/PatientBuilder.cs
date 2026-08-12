using App.Onion.Application.Dtos;
using App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using YourNamespace.Domain;

public class PatientBuilder
{
    private readonly IMedicalRecordNumberGenerator _medicalRecordNumberGenerator;
    private Name _firstName;
    private Name _lastName;
    private DateOfBirth _dateOfBirth;
    private Gender _gender;
    private MedicalRecordNumber _medicalRecordNumber;
    private ContactInfo _contactInformation;
    private EmergencyContact _emergencyContact;

    public PatientBuilder(IMedicalRecordNumberGenerator medicalRecordNumberGenerator)
    {
        _medicalRecordNumberGenerator = medicalRecordNumberGenerator;
    }

    public PatientBuilder WithFirstName(string firstName)
    {
        _firstName = Name.Create(firstName);
        return this;
    }

    public PatientBuilder WithLastName(string lastName)
    {
        _lastName = Name.Create(lastName);
        return this;
    }

    public PatientBuilder WithDateOfBirth(DateOfBirth dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }

    public PatientBuilder WithGender(Gender gender)
    {
        _gender = gender;
        return this;
    }

    public PatientBuilder WithContactInformation(ContactInfoDto contactInformation)
    {
        _contactInformation = new ContactInfo(new PhoneNumber(contactInformation.PhoneNumber), new EmailAddress(contactInformation.EmailAddress));
        return this;
    }

    public PatientBuilder WithEmergencyContact(EmergencyContactDto emergencyContactDto)
    {
        _emergencyContact = new EmergencyContact(Name.Create(emergencyContactDto.EmergencyContactName), new PhoneNumber(emergencyContactDto.EmergencyContactPhoneNumber));
        return this;
    }

    public async Task<Patient> Build()
    {
        // Validações para garantir que os campos obrigatórios estão definidos
        if (_firstName == null)
        {
            throw new InvalidOperationException("First name must be provided.");
        }
        
        if (_lastName == null)
        {
            throw new InvalidOperationException("Last name must be provided.");
        }
        
        if (_dateOfBirth == null)
        {
            throw new InvalidOperationException("Date of birth must be provided.");
        }
        
        if (_gender == null)
        {
            throw new InvalidOperationException("Gender must be provided.");
        }
        
        if (_medicalRecordNumber == null)
        {
            _medicalRecordNumber = await _medicalRecordNumberGenerator.GenerateMedicalRecordNumber();
        }

        return new Patient(
            _firstName,
            _lastName,
            _dateOfBirth,
            _gender,
            _medicalRecordNumber,
            _contactInformation,
            _emergencyContact);
    }
}
