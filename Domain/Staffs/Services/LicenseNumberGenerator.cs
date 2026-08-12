using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.Staffs;

public class LicenseNumberGenerator : ILicenseNumberGenerator
{
    private static readonly Random Random = new();

    /// <summary>
    ///     Generates a new LicenseNumber based on the user's role.
    /// </summary>
    /// <param name="userRole">The user's role, used to generate the license number.</param>
    /// <returns>A new instance of LicenseNumber.</returns>
    public LicenseNumber GenerateLicenseNumber(UserRole userRole)
    {
        if (userRole == null)
            throw new ArgumentNullException(nameof(userRole), "User role cannot be null.");

        var roleInitial = GetRoleInitial(userRole);
        var randomDigits = GenerateRandomDigits(4);

        return new LicenseNumber($"{roleInitial}{randomDigits}");
    }

    /// <summary>
    ///     Retrieves the initial letter of the user role.
    /// </summary>
    /// <param name="userRole">The user role to extract the initial from.</param>
    /// <returns>The initial letter of the role as a char.</returns>
    private char GetRoleInitial(UserRole userRole)
    {
        return userRole.ToString()[0];
    }

    /// <summary>
    ///     Generates a string of random digits of a specified length.
    /// </summary>
    /// <param name="length">The length of the digit string to generate.</param>
    /// <returns>A string containing random digits.</returns>
    private string GenerateRandomDigits(int length)
    {
        return new string(Enumerable.Range(0, length)
            .Select(_ => (char)Random.Next('0', '9' + 1))
            .ToArray());
    }
}