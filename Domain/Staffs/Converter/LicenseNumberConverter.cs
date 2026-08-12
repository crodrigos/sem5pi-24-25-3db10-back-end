using dddnet8.Domain.Staffs.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.Staffs.Converter;

/// <summary>
///     Converts a LicenseNumber value object to a string and vice versa for EF Core entity storage.
/// </summary>
public class LicenseNumberConverter : ValueConverter<LicenseNumber, string>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LicenseNumberConverter" /> class.
    ///     Converts from <see cref="LicenseNumber" /> to string and from string back to <see cref="LicenseNumber" />.
    /// </summary>
    public LicenseNumberConverter()
        : base(
            licenseNumber => ConvertToString(licenseNumber),
            str => ConvertToLicenseNumber(str))
    {
    }

    /// <summary>
    ///     Converts a <see cref="LicenseNumber" /> to a string.
    /// </summary>
    /// <param name="licenseNumber">The LicenseNumber object to convert.</param>
    /// <returns>The string representation of the LicenseNumber.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="licenseNumber" /> is null.</exception>
    private static string ConvertToString(LicenseNumber licenseNumber)
    {
        if (licenseNumber == null)
            throw new ArgumentNullException(nameof(licenseNumber), "LicenseNumber cannot be null.");

        return licenseNumber.ToString();
    }

    /// <summary>
    ///     Converts a string to a <see cref="LicenseNumber" />.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>A LicenseNumber instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is null or empty.</exception>
    private static LicenseNumber ConvertToLicenseNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), "LicenseNumber string cannot be null or empty.");

        return LicenseNumber.FromString(value);
    }
}