using App.Onion.Application.Dtos;
using NUnit.Framework;
using System;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using YourNamespace.Domain;

namespace dddnet8.Domain.SystemUsers.Tests
{
    [TestFixture]
    public class ContactInfoTests
    {
        /// <summary>
        /// Tests that a valid contact information can be successfully created.
        /// </summary>
        [Test]
        public void Constructor_ValidContactInfo_ShouldCreateContactInfo()
        {
            // Arrange
            var phoneNumber = PhoneNumber.Create("00351987654321");
            var emailAddress = EmailAddress.Create("test@example.com");

            // Act
            var contactInfo = new ContactInfo(phoneNumber, emailAddress);

            // Assert
            Assert.That(contactInfo, Is.Not.Null);
            Assert.That(contactInfo.PhoneNumber, Is.EqualTo(phoneNumber));
            Assert.That(contactInfo.EmailAddress, Is.EqualTo(emailAddress));
        }

        /// <summary>
        /// Tests that passing a null phone number to the constructor throws an ArgumentNullException.
        /// </summary>
        [Test]
        public void Constructor_NullPhoneNumber_ThrowsArgumentNullException()
        {
            // Arrange
            var emailAddress = EmailAddress.Create("test@example.com");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ContactInfo(null, emailAddress));
            Assert.That(ex.ParamName, Is.EqualTo("phoneNumber"));
        }

        /// <summary>
        /// Tests that passing a null email address to the constructor throws an ArgumentNullException.
        /// </summary>
        [Test]
        public void Constructor_NullEmailAddress_ThrowsArgumentNullException()
        {
            // Arrange
            var phoneNumber = PhoneNumber.Create("00351987654321");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ContactInfo(phoneNumber, null));
            Assert.That(ex.ParamName, Is.EqualTo("emailAddress"));
        }

        /// <summary>
        /// Tests that a valid ContactInfoDto creates a ContactInfo object.
        /// </summary>
        [Test]
        public void Create_ValidContactInfoDto_ShouldCreateContactInfo()
        {
            // Arrange
            var contactInfoDto = new ContactInfoDto("00351987654321", "test@example.com");
           
            // Act
            var contactInfo = ContactInfo.Create(contactInfoDto);

            // Assert
            Assert.That(contactInfo, Is.Not.Null);
            Assert.That(contactInfo.PhoneNumber.Number, Is.EqualTo("00351987654321"));
            Assert.That(contactInfo.EmailAddress.ToString(), Is.EqualTo("test@example.com"));
        }

        /// <summary>
        /// Tests that an invalid ContactInfoDto throws an ArgumentException when creating ContactInfo.
        /// </summary>
        [Test]
        public void Create_InvalidContactInfoDto_ThrowsArgumentException()
        {
            // Arrange
            var contactInfoDto = new ContactInfoDto("invalid-phone", "invalid-email");
            
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => ContactInfo.Create(contactInfoDto));
            Assert.That(ex.Message, Is.EqualTo("Phone number must start with a valid Portuguese mobile prefix and have 9 digits."));
        }

        /// <summary>
        /// Tests that two identical ContactInfo objects are considered equal.
        /// </summary>
        [Test]
        public void Equals_SameContactInfo_ShouldReturnTrue()
        {
            // Arrange
            var phoneNumber = PhoneNumber.Create("00351987654321");
            var emailAddress = EmailAddress.Create("test@example.com");
            var contactInfo1 = new ContactInfo(phoneNumber, emailAddress);
            var contactInfo2 = new ContactInfo(phoneNumber, emailAddress);

            // Act
            bool areEqual = contactInfo1.Equals(contactInfo2);

            // Assert
            Assert.That(areEqual, Is.True);
        }

        /// <summary>
        /// Tests that two different ContactInfo objects are considered unequal.
        /// </summary>
        [Test]
        public void Equals_DifferentContactInfo_ShouldReturnFalse()
        {
            // Arrange
            var contactInfo1 = new ContactInfo(PhoneNumber.Create("00351987654321"), EmailAddress.Create("test@example.com"));
            var contactInfo2 = new ContactInfo(PhoneNumber.Create("00351912345678"), EmailAddress.Create("test2@example.com"));

            // Act
            bool areEqual = contactInfo1.Equals(contactInfo2);

            // Assert
            Assert.That(areEqual, Is.False);
        }

        /// <summary>
        /// Tests that the ToString method returns the expected formatted string representation.
        /// </summary>
        [Test]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var contactInfo = new ContactInfo(PhoneNumber.Create("00351987654321"), EmailAddress.Create("test@example.com"));

            // Act
            string result = contactInfo.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("test@example.com;00351987654321")); // Check for expected format
        }

        /// <summary>
        /// Tests that a valid formatted string creates a ContactInfo object.
        /// </summary>
        [Test]
        public void FromString_ValidString_ShouldCreateContactInfo()
        {
            // Arrange
            string contactInfoString = "test@example.com; 00351987654321";

            // Act
            var contactInfo = ContactInfo.FromString(contactInfoString);

            // Assert
            Assert.That(contactInfo, Is.Not.Null);
            Assert.That(contactInfo.EmailAddress.ToString(), Is.EqualTo("test@example.com"));
            Assert.That(contactInfo.PhoneNumber.Number, Is.EqualTo("00351987654321"));
        }

        /// <summary>
        /// Tests that an invalid format for the string throws a FormatException.
        /// </summary>
        [Test]
        public void FromString_InvalidFormat_ThrowsFormatException()
        {
            // Arrange
            string contactInfoString = "invalid-format";

            // Act & Assert
            var ex = Assert.Throws<FormatException>(() => ContactInfo.FromString(contactInfoString));
            Assert.That(ex.Message, Is.EqualTo("Contact information must be in the format 'Email;Phone'."));
        }

        /// <summary>
        /// Tests that updating contact information with a valid DTO updates the fields correctly.
        /// </summary>
        [Test]
        public void UpdateContactInformation_ValidDto_ShouldUpdateContactInfo()
        {
            // Arrange
            var initialContactInfo = new ContactInfo(PhoneNumber.Create("00351987654321"), EmailAddress.Create("test@example.com"));
            var newContactInfoDto = new ContactInfoDto("00351999999999", "new@example.com");
            
            // Act
            initialContactInfo.UpdateContactInformation(newContactInfoDto);

            // Assert
            Assert.That(initialContactInfo.PhoneNumber.Number, Is.EqualTo("00351999999999"));
            Assert.That(initialContactInfo.EmailAddress.ToString(), Is.EqualTo("new@example.com"));
        }
    }
}
