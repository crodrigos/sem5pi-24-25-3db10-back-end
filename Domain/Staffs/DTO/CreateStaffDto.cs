using dddnet8.Domain.Patients.DTO;

namespace dddnet8.Domain.Staffs.DTO;

/// <summary>
///     DTO for creating a new staff member.
/// </summary>
public class CreateStaffDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public ContactInfoDto ContactInformation { get; set; }
    public string Role { get; set; }

    public CreateStaffDto() { }

    public CreateStaffDto(string firstName, string lastName, string specialization,
        ContactInfoDto contactInformation, string role)
    {
        FirstName = firstName;
        LastName = lastName;
        Specialization = specialization;
        ContactInformation = contactInformation;
        Role = role;
    }
}
