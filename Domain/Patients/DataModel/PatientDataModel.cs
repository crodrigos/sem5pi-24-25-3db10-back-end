using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Shared;
using SurgicalManagement.Domain.Common;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using YourNamespace.Domain;

namespace dddnet8.Domain.Patients.DataModel;

public class PatientDataModel : Entity<Guid>
{
    public Guid Id { get; init; }
    public Name FirstName { get; set; }       
    public Name LastName { get; set; }
    public Name FullName { get; private set; }
    public DateOfBirth DateOfBirth { get; set; }   
    public Gender Gender { get; set; }             
    public MedicalRecordNumber MedicalRecordNumber { get; private set; }  
    public ContactInfo ContactInformation { get; set; }
    public EmergencyContact EmergencyContact { get; private set; }
    
    public DeletionStatus DeletionStatus { get; private set; } 
    
    protected PatientDataModel() : base(Guid.NewGuid()) {DeletionStatus = DeletionStatus.Create(false);}

    public PatientDataModel(Guid? id, Name firstName, Name lastName, DateOfBirth dateOfBirth, Gender gender, 
        MedicalRecordNumber medicalRecordNumber, ContactInfo contactInformation, 
        EmergencyContact emergencyContact, DeletionStatus deletionStatus) 
        : base(id ?? Guid.NewGuid()) 
    {
        Id = id ?? Guid.NewGuid();  
        FirstName = firstName;
        LastName = lastName;
        FullName = Name.Create($"{firstName} {lastName}"); 
        DateOfBirth = dateOfBirth;
        Gender = gender;
        MedicalRecordNumber = medicalRecordNumber;
        ContactInformation = contactInformation;
        EmergencyContact = emergencyContact;
        DeletionStatus = deletionStatus;
    }

}