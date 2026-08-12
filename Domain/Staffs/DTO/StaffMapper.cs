using dddnet8.Domain.Patients.DTO;

namespace dddnet8.Domain.Staffs.DTO;

public static class StaffMapper
{
    /// <summary>
    ///     Converts a Staff entity to a StaffDto.
    /// </summary>
    /// <param name="staff">The staff entity.</param>
    /// <returns>The corresponding StaffDto.</returns>
    public static StaffDto ToDto(Staff staff)
    {
        return new StaffDto(
            staff.FirstName.Value + " " + staff.LastName.Value,
            staff.Specialization.Name.Value,
            staff.LicenseNumber.Value,
            new ContactInfoDto(staff.ContactInfo.PhoneNumber.ToString(), staff.ContactInfo.EmailAddress.GetFullEmail())
        );
    }

    /// <summary>
    ///     Converts a list of Staff entities to a list of StaffDto.
    /// </summary>
    /// <param name="staffList">The list of staff entities.</param>
    /// <returns>A list of corresponding StaffDto objects.</returns>
    public static List<StaffDto> ToDtoList(IEnumerable<Staff> staffList)
    {
        return staffList.Select(ToDto).ToList();
    }
}