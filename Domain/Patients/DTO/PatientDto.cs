using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using YourNamespace.Domain;

namespace App.Onion.Application.Dtos;

public class PatientDto(string fullName, string gender, string dateOfBirth, string medicalRecordNumber, ContactInfoDto contactInfoDto, EmergencyContactDto emergencyContactDto)
{ 
    public string FullName { get; set; } = fullName;
    public string Gender { get; set; } = gender;
    public string DateOfBirth { get; set; } = dateOfBirth;
    public string MedicalRecordNumber { get; set; } = medicalRecordNumber;

    public ContactInfoDto ContactInfoDto { get; set; } = contactInfoDto;

    public EmergencyContactDto EmergencyContactDto { get; set; } = emergencyContactDto;
}