using NUnit.Framework;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Tests
{
    [TestFixture]
    public class LicenseNumberTests
    {
        /// <summary>
        /// Testa a criação de um LicenseNumber válido.
        /// </summary>
        [Test]
        public void Constructor_ShouldCreateLicenseNumber_WhenValidValueProvided()
        {
            // Arrange
            var validLicense = "A1234";

            // Act
            var licenseNumber = new LicenseNumber(validLicense);

            // Assert
            Assert.That(licenseNumber, Is.Not.Null);
            Assert.That(licenseNumber.Value, Is.EqualTo(validLicense));
        }

        /// <summary>
        /// Testa que uma exceção é lançada quando o valor fornecido é nulo.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowInvalidLicenseNumberException_WhenValueIsNull()
        {
            // Arrange
            string nullValue = null;

            // Act & Assert
            var ex = Assert.Throws<InvalidLicenseNumberException>(() => new LicenseNumber(nullValue));
            Assert.That(ex.Message, Is.EqualTo("Invalid license number format. Expected format: 'A1234'. (Parameter 'invalidValue')"));
        }

        /// <summary>
        /// Testa que uma exceção é lançada quando o valor fornecido é uma string vazia.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowInvalidLicenseNumberException_WhenValueIsEmpty()
        {
            // Arrange
            var emptyValue = string.Empty;

            // Act & Assert
            var ex = Assert.Throws<InvalidLicenseNumberException>(() => new LicenseNumber(emptyValue));
            Assert.That(ex.Message, Is.EqualTo("Invalid license number format. Expected format: 'A1234'. (Parameter 'invalidValue')"));
        }

        /// <summary>
        /// Testa que uma exceção é lançada quando o valor fornecido não corresponde ao formato esperado.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowInvalidLicenseNumberException_WhenValueIsInvalidFormat()
        {
            // Arrange
            var invalidValue1 = "12345"; // apenas números, sem letra no início
            var invalidValue2 = "AB123"; // duas letras no início

            // Act & Assert para valor numérico sem letra no início
            var ex1 = Assert.Throws<InvalidLicenseNumberException>(() => new LicenseNumber(invalidValue1));
            Assert.That(ex1.Message, Is.EqualTo("Invalid license number format. Expected format: 'A1234'. (Parameter 'invalidValue')"));

            // Act & Assert para valor com duas letras
            var ex2 = Assert.Throws<InvalidLicenseNumberException>(() => new LicenseNumber(invalidValue2));
            Assert.That(ex2.Message, Is.EqualTo("Invalid license number format. Expected format: 'A1234'. (Parameter 'invalidValue')"));
        }

        /// <summary>
        /// Testa que duas instâncias de LicenseNumber com o mesmo valor são iguais.
        /// </summary>
        [Test]
        public void Equals_ShouldReturnTrue_WhenValuesAreTheSame()
        {
            // Arrange
            var licenseNumber1 = new LicenseNumber("A1234");
            var licenseNumber2 = new LicenseNumber("A1234");

            // Act & Assert
            Assert.That(licenseNumber1, Is.EqualTo(licenseNumber2));
        }

        /// <summary>
        /// Testa que duas instâncias de LicenseNumber com valores diferentes não são iguais.
        /// </summary>
        [Test]
        public void Equals_ShouldReturnFalse_WhenValuesAreDifferent()
        {
            // Arrange
            var licenseNumber1 = new LicenseNumber("A1234");
            var licenseNumber2 = new LicenseNumber("B5678");

            // Act & Assert
            Assert.That(licenseNumber1, Is.Not.EqualTo(licenseNumber2));
        }
    }
}
