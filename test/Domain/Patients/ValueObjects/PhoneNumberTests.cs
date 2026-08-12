using NUnit.Framework;
using System;

namespace App.Onion.Domain.V.O.Patient.Tests
{
    [TestFixture]
    public class PhoneNumberTests
    {
        /// <summary>
        /// Tests that a valid phone number is successfully created.
        /// </summary>
        [Test]
        public void Constructor_ValidPhoneNumber_ShouldCreatePhoneNumber()
        {
            // Arrange
            string validNumber = "00351987654321";

            // Act
            var phoneNumber = new PhoneNumber(validNumber);

            // Assert
            Assert.That(phoneNumber, Is.Not.Null);
            Assert.That(phoneNumber.Number, Is.EqualTo(validNumber));
        }

        /// <summary>
        /// Tests that an invalid phone number throws an ArgumentException.
        /// </summary>
        [Test]
        public void Constructor_InvalidPhoneNumber_ThrowsArgumentException()
        {
            // Arrange
            string invalidNumber = "123456789"; // Invalid as it does not start with valid codes

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new PhoneNumber(invalidNumber));
            Assert.That(ex.Message, Is.EqualTo("Phone number must start with a valid Portuguese country code."));
        }

        /// <summary>
        /// Tests that a valid phone number is successfully created using the Create method.
        /// </summary>
        [Test]
        public void Create_ValidPhoneNumber_ShouldCreatePhoneNumber()
        {
            // Arrange
            string validNumber = "351987654321";

            // Act
            var phoneNumber = PhoneNumber.Create(validNumber);

            // Assert
            Assert.That(phoneNumber, Is.Not.Null);
            Assert.That(phoneNumber.Number, Is.EqualTo(validNumber));
        }

        /// <summary>
        /// Tests that passing a null phone number to the Create method throws an ArgumentException.
        /// </summary>
        [Test]
        public void Create_NullPhoneNumber_ThrowsArgumentException()
        {
            // Arrange
            string nullNumber = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(nullNumber));
            Assert.That(ex.Message, Is.EqualTo("Phone number cannot be null or empty."));
        }

        /// <summary>
        /// Tests that passing an empty phone number to the Create method throws an ArgumentException.
        /// </summary>
        [Test]
        public void Create_EmptyPhoneNumber_ThrowsArgumentException()
        {
            // Arrange
            string emptyNumber = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(emptyNumber));
            Assert.That(ex.Message, Is.EqualTo("Phone number cannot be null or empty."));
        }

        /// <summary>
        /// Tests that two instances of PhoneNumber with the same number are considered equal.
        /// </summary>
        [Test]
        public void Equals_SamePhoneNumber_ShouldReturnTrue()
        {
            // Arrange
            var phoneNumber1 = new PhoneNumber("00351987654321");
            var phoneNumber2 = new PhoneNumber("00351987654321");

            // Act
            bool areEqual = phoneNumber1.Equals(phoneNumber2);

            // Assert
            Assert.That(areEqual, Is.True);
        }

        /// <summary>
        /// Tests that two instances of PhoneNumber with different numbers are not considered equal.
        /// </summary>
        [Test]
        public void Equals_DifferentPhoneNumber_ShouldReturnFalse()
        {
            // Arrange
            var phoneNumber1 = new PhoneNumber("00351987654321");
            var phoneNumber2 = new PhoneNumber("00351912345678");

            // Act
            bool areEqual = phoneNumber1.Equals(phoneNumber2);

            // Assert
            Assert.That(areEqual, Is.False);
        }

        /// <summary>
        /// Tests that two instances of PhoneNumber with the same number return the same hash code.
        /// </summary>
        [Test]
        public void GetHashCode_SamePhoneNumber_ShouldReturnSameHashCode()
        {
            // Arrange
            var phoneNumber1 = new PhoneNumber("351987654321");
            var phoneNumber2 = new PhoneNumber("351987654321");

            // Act
            int hash1 = phoneNumber1.GetHashCode();
            int hash2 = phoneNumber2.GetHashCode();

            // Assert
            Assert.That(hash1, Is.EqualTo(hash2));
        }

        /// <summary>
        /// Tests that ToString returns the correct phone number string representation.
        /// </summary>
        [Test]
        public void ToString_ShouldReturnPhoneNumberString()
        {
            // Arrange
            var phoneNumber = new PhoneNumber("351987654321");

            // Act
            string result = phoneNumber.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("351987654321"));
        }
    }
}
