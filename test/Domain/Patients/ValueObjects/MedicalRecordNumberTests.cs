using NUnit.Framework;
using System;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class MedicalRecordNumberTests
    {
        /// <summary>
        /// Tests that a valid Medical Record Number is created successfully.
        /// </summary>
        [Test]
        public void Create_ShouldReturnMedicalRecordNumber_WhenValidValueProvided()
        {
            // Arrange
            var validNumber = "123456789012";

            // Act
            var medicalRecordNumber = MedicalRecordNumber.Create(validNumber);

            // Assert
            Assert.That(medicalRecordNumber, Is.Not.Null);
            Assert.That(medicalRecordNumber.ToString(), Is.EqualTo(validNumber));
        }

        /// <summary>
        /// Tests that an exception is thrown when a null value is provided.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsNull()
        {
            // Arrange
            string nullValue = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new MedicalRecordNumber(nullValue));
            Assert.That(ex.Message, Is.EqualTo("Medical Record Number must be a positive integer. (Parameter 'value')"));
        }

        /// <summary>
        /// Tests that an exception is thrown when an empty string is provided.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsEmpty()
        {
            // Arrange
            var emptyValue = string.Empty;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new MedicalRecordNumber(emptyValue));
            Assert.That(ex.Message, Is.EqualTo("Medical Record Number must be a positive integer. (Parameter 'value')"));
        }

        /// <summary>
        /// Tests that an exception is thrown when the value is "0".
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenValueIsZero()
        {
            // Arrange
            var zeroValue = "0";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new MedicalRecordNumber(zeroValue));
            Assert.That(ex.Message, Is.EqualTo("Medical Record Number must be a positive integer. (Parameter 'value')"));
        }

        /// <summary>
        /// Tests that an exception is thrown when the length of the value is not 12.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrowArgumentException_WhenValueLengthIsNot12()
        {
            // Arrange
            var shortValue = "12345678901"; // 11 digits
            var longValue = "1234567890123"; // 13 digits

            // Act & Assert for short value
            var exShort = Assert.Throws<ArgumentException>(() => new MedicalRecordNumber(shortValue));
            Assert.That(exShort.Message, Is.EqualTo("Medical Record Number must be exactly 12 digits long. (Parameter 'value')"));

            // Act & Assert for long value
            var exLong = Assert.Throws<ArgumentException>(() => new MedicalRecordNumber(longValue));
            Assert.That(exLong.Message, Is.EqualTo("Medical Record Number must be exactly 12 digits long. (Parameter 'value')"));
        }
    }
}
