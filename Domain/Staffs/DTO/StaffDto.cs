using dddnet8.Domain.Patients.DTO;

namespace dddnet8.Domain.Staffs.DTO;

public class StaffDto(string fullName, string specialization, string licenseNumber, ContactInfoDto contactInfoDto)
{
    public string FullName { get; set; } = fullName;
    public string Specialization { get; set; } = specialization;
    public string LicenseNumber { get; set; } = licenseNumber;
    public ContactInfoDto ContactInfoDto { get; set; } = contactInfoDto;
}
