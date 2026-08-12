using dddnet8.Domain.Patients.DTO;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using YourNamespace.Domain;

namespace App.Onion.Application.Dtos;

public class CreatePatientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ContactInfoDto ContactInformation { get; set; }
    public EmergencyContactDto EmergencyContact { get; set; }
    
}