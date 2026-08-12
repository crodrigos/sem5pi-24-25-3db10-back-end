using dddnet8.Domain.Staffs.V.O;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.Staffs.Interfaces;

/// <summary>
///     Defines the contract for generating license numbers for staff members based on their user role.
/// </summary>
public interface ILicenseNumberGenerator
{
    /// <summary>
    ///     Generates a license number for a staff member based on their user role.
    /// </summary>
    /// <param name="userRole">The role of the user (e.g., Doctor, Nurse) for which the license number will be generated.</param>
    /// <returns>A LicenseNumber object representing the generated license number.</returns>
    LicenseNumber GenerateLicenseNumber(UserRole userRole);
}