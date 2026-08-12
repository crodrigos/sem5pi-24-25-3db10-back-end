using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Specializations.DTO;

namespace dddnet8.Domain.Staffs.DTO;

/// <summary>
///     Represents criteria for filtering or searching staff members.
///     All fields are optional and can be used to build dynamic queries.
/// </summary>
public class StaffCriteria
{
    /// <summary>
    ///     Gets or sets the first name of the staff member.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the last name of the staff member.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     Gets or sets the full name of the staff member.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    ///     Gets or sets the email address of the staff member.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Gets or sets the phone number of the staff member.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     Gets or sets the license number of the staff member.
    /// </summary>
    public string? LicenseNumber { get; set; }

    /// <summary>
    ///     Gets or sets the specialization of the staff member.
    /// </summary>
    public string? SpecializationName { get; set; }

    /// <summary>
    ///     Gets or sets the contact information of the staff member.
    /// </summary>
    public ContactInfoDto? ContactInformation { get; set; }
    
    /// <summary>
    ///     Gets or sets the ID of the specialization.
    /// </summary>
    public SpecializationDto? Specialization { get; set; }
}