using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.Staffs.Converter;

/// <summary>
///     Converts a Specialization value object to a string and vice versa for EF Core entity storage.
/// </summary>
public class SpecializationConverter : ValueConverter<Specialization, string>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LicenseNumberConverter" /> class.
    ///     Converts from <see cref="LicenseNumber" /> to string and from string back to <see cref="LicenseNumber" />.
    /// </summary>
    public SpecializationConverter()
        : base(
            specialization => ConvertToString(specialization),
            str => ConvertToSpecialization(str))
    {
    }

    /// <summary>
    ///     Converts a <see cref="LicenseNumber" /> to a string.
    /// </summary>
    /// <param name="specialization">The Specialization object to convert.</param>
    /// <returns>The string representation of the Specialization.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="specialization" /> is null.</exception>
    private static string ConvertToString(Specialization specialization)
    {
        if (specialization == null)
            throw new ArgumentNullException(nameof(specialization), "Specialization cannot be null.");

        return specialization.ToString();
    }

    /// <summary>
    ///     Converts a string to a <see cref="Specialization" />.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>A Specialization instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is null or empty.</exception>
    private static Specialization ConvertToSpecialization(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), "Specialization string cannot be null or empty.");

        return Specialization.FromString(value);
    }
}