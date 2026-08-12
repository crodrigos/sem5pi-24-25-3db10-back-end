using NUnit.Framework;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using SurgicalManagement.Domain.Domain;
using System;

namespace dddnet8.Tests
{
    /// <summary>
    /// Unit tests for the LicenseNumberGenerator class, which generates license numbers for staff based on their role.
    /// </summary>
    [TestFixture]
    public class LicenseNumberGeneratorTests
    {
        /// <summary>
        /// Instance of LicenseNumberGenerator to be tested.
        /// </summary>
        private LicenseNumberGenerator _licenseNumberGenerator;

        /// <summary>
        /// Sets up the test environment by initializing the LicenseNumberGenerator instance.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _licenseNumberGenerator = new LicenseNumberGenerator();
        }

        /// <summary>
        /// Tests if GenerateLicenseNumber generates a license number with default UserRole value.
        /// </summary>
        [Test]
        public void GenerateLicenseNumber_ShouldGenerateLicenseNumber_WithDefaultUserRole()
        {
            // Arrange: Use default(UserRole) to simulate a default or unassigned role scenario.
            var defaultUserRole = default(UserRole); // Geralmente representa o valor 0 ou não inicializado.

            // Act: Generate the license number with default UserRole.
            var licenseNumber = _licenseNumberGenerator.GenerateLicenseNumber(defaultUserRole);

            // Assert: Verify that the generated license number is in the expected format, e.g., "A1234".
            // Substitua "A" pela inicial padrão de acordo com o comportamento atual do seu método.
            Assert.That(licenseNumber.Value, Does.Match(@"^[A-Z]\d{4}$"));
        }



        /// <summary>
        /// Tests if GenerateLicenseNumber generates a license number with the correct format for the provided role.
        /// </summary>
        [Test]
        public void GenerateLicenseNumber_ShouldGenerateLicenseNumberWithCorrectFormat()
        {
            // Arrange: Define a user role to generate a license number
            var userRole = UserRole.Doctor;

            // Act: Generate the license number
            var licenseNumber = _licenseNumberGenerator.GenerateLicenseNumber(userRole);

            // Assert: Verify that the generated license number starts with the initial of the role and has 4 digits
            Assert.That(licenseNumber.Value, Does.Match(@"^D\d{4}$"));
        }

        /// <summary>
        /// Tests if GenerateLicenseNumber generates unique license numbers when called multiple times.
        /// </summary>
        [Test]
        public void GenerateLicenseNumber_ShouldGenerateUniqueLicenseNumbers()
        {
            // Arrange: Define a user role
            var userRole = UserRole.Nurse;

            // Act: Generate multiple license numbers
            var licenseNumber1 = _licenseNumberGenerator.GenerateLicenseNumber(userRole);
            var licenseNumber2 = _licenseNumberGenerator.GenerateLicenseNumber(userRole);

            // Assert: Verify that the generated license numbers are unique
            Assert.That(licenseNumber1.Value, Is.Not.EqualTo(licenseNumber2.Value));
        }

        /// <summary>
        /// Tests if GenerateLicenseNumber correctly assigns the initial letter for different roles.
        /// </summary>
        [Test]
        public void GenerateLicenseNumber_ShouldAssignCorrectInitialForDifferentRoles()
        {
            // Act & Assert: Check if the generated license number starts with the correct initial for each role
            Assert.That(_licenseNumberGenerator.GenerateLicenseNumber(UserRole.Doctor).Value, Does.StartWith("D"));
            Assert.That(_licenseNumberGenerator.GenerateLicenseNumber(UserRole.Nurse).Value, Does.StartWith("N"));
            Assert.That(_licenseNumberGenerator.GenerateLicenseNumber(UserRole.Technician).Value, Does.StartWith("T"));
        }
    }
}
