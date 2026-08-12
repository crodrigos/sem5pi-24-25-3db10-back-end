using dddnet8.Domain.Patients.V.O;
using NUnit.Framework;
using dddnet8.Domain.Patients.VO.Name;

namespace SurgicalManagement.Tests.Domain.Patients
{
    [TestFixture]
    public class NameTests
    {
        /// <summary>
        /// Tests if the Create method returns a Name instance when given a valid value.
        /// </summary>
        [Test]
        public void Create_ShouldReturnName_WhenValueIsValid()
        {
            // Arrange
            var validName = "John Doe";

            // Act
            var name = Name.Create(validName);

            // Assert
            Assert.That(name, Is.EqualTo(Name.Create(validName)));
        }

        /// <summary>
        /// Tests if the Create method throws an ArgumentException when the value is null.
        /// </summary>
        [Test]
        public void Create_ShouldThrowArgumentException_WhenValueIsNull()
        {
            // Arrange
            string invalidName = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Name.Create(invalidName));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be null or empty."));
        }

        /// <summary>
        /// Tests if the Create method throws an ArgumentException when the value is an empty string.
        /// </summary>
        [Test]
        public void Create_ShouldThrowArgumentException_WhenValueIsEmpty()
        {
            // Arrange
            string invalidName = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Name.Create(invalidName));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be null or empty."));
        }

        /// <summary>
        /// Tests if the Create method throws an ArgumentException when the value consists only of whitespace.
        /// </summary>
        [Test]
        public void Create_ShouldThrowArgumentException_WhenValueIsWhitespace()
        {
            // Arrange
            string invalidName = "   "; // Only whitespace

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Name.Create(invalidName));
            Assert.That(ex.Message, Is.EqualTo("Name cannot have only whitespace."));
        }

        /// <summary>
        /// Tests if the Equals method returns true when two Name instances are equal.
        /// </summary>
        [Test]
        public void Equals_ShouldReturnTrue_WhenNamesAreEqual()
        {
            // Arrange
            var name1 = Name.Create("John Doe");
            var name2 = Name.Create("John Doe");

            // Act
            var areEqual = name1.Equals(name2);

            // Assert
            Assert.That(areEqual, Is.True);
        }

        /// <summary>
        /// Tests if the Equals method returns false when two Name instances are not equal.
        /// </summary>
        [Test]
        public void Equals_ShouldReturnFalse_WhenNamesAreNotEqual()
        {
            // Arrange
            var name1 = Name.Create("John Doe");
            var name2 = Name.Create("Jane Doe");

            // Act
            var areEqual = name1.Equals(name2);

            // Assert
            Assert.That(areEqual, Is.False);
        }

        /// <summary>
        /// Tests if the ToString method returns the correct string representation of the Name instance.
        /// </summary>
        [Test]
        public void ToString_ShouldReturnValue_WhenCalled()
        {
            // Arrange
            var name = Name.Create("John Doe");

            // Act
            var nameString = name.ToString();

            // Assert
            Assert.That(nameString, Is.EqualTo("John Doe"));
        }
    }
}
