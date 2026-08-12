using System.Text.RegularExpressions;
using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Staffs.V.O
{
    /// <summary>
    /// Represents a license number value object.
    /// </summary>
    public class LicenseNumber : ValueObject
    {
        private const string LicensePattern = @"^[A-Z]\d{4}$";

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseNumber"/> class.
        /// </summary>
        /// <param name="value">The license number value.</param>
        /// <exception cref="InvalidLicenseNumberException">Thrown when the license number format is invalid.</exception>
        public LicenseNumber(string value)
        {
            Value = !string.IsNullOrWhiteSpace(value) && IsValid(value)
                ? value
                : throw new InvalidLicenseNumberException("Invalid license number format. Expected format: 'A1234'.", value);
        }

        /// <summary>
        /// Gets the value of the license number.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Validates the license number format using a regular expression.
        /// </summary>
        /// <param name="value">The license number to validate.</param>
        /// <returns>True if the license number is valid, otherwise false.</returns>
        private static bool IsValid(string value)
        {
            return Regex.IsMatch(value, LicensePattern);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is LicenseNumber other && Value.Equals(other.Value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Provides atomic values for equality comparison.
        /// </summary>
        /// <returns>The atomic value (license number).</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        /// <summary>
        /// Returns the string representation of the license number.
        /// </summary>
        /// <returns>The license number as a string.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Factory method to create a <see cref="LicenseNumber"/> instance from a string.
        /// </summary>
        /// <param name="licenseNumber">The license number string.</param>
        /// <returns>A new <see cref="LicenseNumber"/> instance.</returns>
        public static LicenseNumber FromString(string licenseNumber)
        {
            return new LicenseNumber(licenseNumber);
        }
    }

    /// <summary>
    /// Exception thrown when a license number format is invalid.
    /// </summary>
    public class InvalidLicenseNumberException : ArgumentException
    {
        public InvalidLicenseNumberException(string message, string invalidValue) 
            : base(message, nameof(invalidValue))
        {
            InvalidValue = invalidValue;
        }

        /// <summary>
        /// Gets the invalid license number value that caused the exception.
        /// </summary>
        public string InvalidValue { get; }
    }
}
