using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Shared;
using SurgicalManagement.Domain.Common;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using YourNamespace.Domain;

public class Patient 
{
    public Name FirstName { get; set; }       
    public Name LastName { get; set; }
    public Name FullName { get; private set; }
    public DateOfBirth DateOfBirth { get; set; }   
    public Gender Gender { get; set; }             
    public MedicalRecordNumber MedicalRecordNumber { get; private set; }  
    public ContactInfo ContactInformation { get; set; } 
    
    public EmergencyContact EmergencyContact { get; set; }
    
    public DeletionStatus DeletionStatus { get; private set; } 
    
    protected Patient() {DeletionStatus = DeletionStatus.Create(false);}

    public Patient(Name firstName, Name lastName, DateOfBirth dateOfBirth, Gender gender, 
        MedicalRecordNumber medicalRecordNumber, ContactInfo contactInformation, 
        EmergencyContact emergencyContact, DeletionStatus? deletionStatus = null) 
    {
        FirstName = firstName;
        LastName = lastName;
        FullName =  Name.Create($"{firstName} {lastName}"); 
        DateOfBirth = dateOfBirth;
        Gender = gender;
        MedicalRecordNumber = medicalRecordNumber;
        ContactInformation = contactInformation;
        EmergencyContact = emergencyContact;
        DeletionStatus = deletionStatus == null ? DeletionStatus.Create(false) : deletionStatus; 
    }

    public void UpdatePatient(PatientCriteria patientCriteria)
    {
        if (patientCriteria.FirstName != null) {UpdateFirstName(Name.Create(patientCriteria.FirstName));}

        if (patientCriteria.LastName != null) {UpdateLastName(Name.Create(patientCriteria.LastName));}

        if (patientCriteria.ContactInformation != null) {ContactInformation.UpdateContactInformation(patientCriteria.ContactInformation);}

        if (patientCriteria.EmergencyContact != null) {EmergencyContact.UpdateEmergencyContact(patientCriteria.EmergencyContact);}

        if (patientCriteria.DateOfBirth != null) {UpdateDateOfBirth((DateTime)patientCriteria.DateOfBirth);}

        if (patientCriteria.Gender != null) {UpdateGender(patientCriteria.Gender);}
    } 
     
    private void UpdateFirstName(Name newFirstName) {FirstName = newFirstName;UpdateFullName();}
    private void UpdateLastName(Name newLastName) {LastName = newLastName;UpdateFullName();}

    private void UpdateFullName() {FullName = Name.Create($"{FirstName} {LastName}");}
    private void UpdateDateOfBirth(DateTime newDateOfBirth) {DateOfBirth = DateOfBirth.Create(newDateOfBirth);}
    private void UpdateGender(string newGender)
    {if (Enum.TryParse<Gender>(newGender, true, out var parsedGender)){Gender = parsedGender;}
        else {throw new ArgumentException($"'{newGender}' não é um valor válido para o gênero.");}
    }
    public void MarkForDeletion(){DeletionStatus = DeletionStatus.Create(true, DateTime.UtcNow);}
    public bool CanDelete() {return DeletionStatus.CanDelete();}
}