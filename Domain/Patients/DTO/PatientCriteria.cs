using App.Onion.Application.Dtos;
using dddnet8.Domain.Patients.DTO;
    public class PatientCriteria
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MedicalRecordNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public ContactInfoDto? ContactInformation { get; set; }
        public EmergencyContactDto? EmergencyContact { get; set; }
        
        // Construtor sem parâmetros (necessário para o model binding)
        public PatientCriteria()
        {
        }

        // Construtor
        public PatientCriteria(
            string? firstName = null,
            string? lastName = null,
            string? fullName = null,
            string? email = null,
            string? phoneNumber = null,
            string? medicalRecordNumber = null,
            DateTime? dateOfBirth = null,
            string? gender = null,
            ContactInfoDto? contactInformation = null,
            EmergencyContactDto? emergencyContact = null)
        {
            FirstName = firstName;
            LastName = lastName;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            MedicalRecordNumber = medicalRecordNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            ContactInformation = contactInformation;
            EmergencyContact = emergencyContact;
        }
    }
