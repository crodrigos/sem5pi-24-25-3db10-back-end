using dddnet8.Domain.Patients.V.O;
using SurgicalManagement.Domain.Common;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using YourNamespace.Domain;
using YourNamespace.GDPR.Entities;

namespace dddnet8.AuditLog.Entities
{
    public class PatientLog : LogEntry
    {
        public Name FirstName { get; private set; }  
        public Name LastName { get; private set; }   
        public Name FullName { get; private set; }   
        public DateOfBirth DateOfBirth { get; private set; } 
        public Gender Gender { get; private set; }   
        public MedicalRecordNumber MedicalRecordNumber { get; private set; } 
        public ContactInfo ContactInformation { get; private set; } 
        public EmergencyContact EmergencyContact { get; private set; } 
        public MedicalCondition? MedicalCondition { get; private set; }
        
        public DeletionStatus DeletionStatus { get; private set; }

        // Construtor protegido para uso do EF (se necessário)
        protected PatientLog() : base("action", "entityType") { }

        public PatientLog(string action,Name firstName, Name lastName, DateOfBirth dateOfBirth, 
                          Gender gender, MedicalRecordNumber medicalRecordNumber, ContactInfo contactInformation, 
                          DeletionStatus deletionStatus, EmergencyContact emergencyContact, 
                          MedicalCondition? medicalCondition = null) 
            : base(action, "Patient")
        {
            FirstName = firstName;
            LastName = lastName; 
            FullName = Name.Create($"{firstName} {lastName}"); 
            DateOfBirth = dateOfBirth; 
            Gender = gender; 
            MedicalRecordNumber = medicalRecordNumber; 
            ContactInformation = contactInformation; 
            EmergencyContact = emergencyContact; 
            MedicalCondition = medicalCondition; 
            DeletionStatus = deletionStatus; 
        }
    }
}
