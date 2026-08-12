using App.Passsword.Encoder;
using NUnit.Framework;

namespace App.Password.Tests
{
    [TestFixture]
    public class PasswordEncoderTests
    {
        private IPasswordEncoder _passwordEncoder;

        [SetUp]
        public void Setup()
        {
            // Initializes the PasswordEncoder instance before each test
            _passwordEncoder = new PasswordEncoder();
        }

        /// <summary>
        /// Tests that the Encode method returns a valid hashed password when a valid password is provided.
        /// </summary>
        [Test]
        public void Encode_ShouldReturnHashedPassword_WhenValidPasswordIsProvided()
        {
            // Arrange
            var password = "my_secure_password";

            // Act
            var hashedPassword = _passwordEncoder.Encode(password);

            // Assert
            Assert.That(hashedPassword, Is.Not.Null);
            Assert.That(hashedPassword, Is.Not.Empty);
            Assert.That(hashedPassword, Does.StartWith("$2a$")); // Verifies that the hash starts with the BCrypt prefix
        }

        /// <summary>
        /// Tests that the Verify method returns true when the correct password is provided.
        /// </summary>
        [Test]
        public void Verify_ShouldReturnTrue_WhenCorrectPasswordIsProvided()
        {
            // Arrange
            var password = "my_secure_password";
            var hashedPassword = _passwordEncoder.Encode(password);

            // Act
            var result = _passwordEncoder.Verify(password, hashedPassword);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that the Verify method returns false when an incorrect password is provided.
        /// </summary>
        [Test]
        public void Verify_ShouldReturnFalse_WhenIncorrectPasswordIsProvided()
        {
            // Arrange
            var password = "my_secure_password";
            var hashedPassword = _passwordEncoder.Encode(password);
            var wrongPassword = "wrong_password";

            // Act
            var result = _passwordEncoder.Verify(wrongPassword, hashedPassword);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that the Verify method returns false when an invalid hashed password is provided.
        /// </summary>
        [Test]
        public void Verify_ShouldReturnFalse_WhenHashedPasswordIsInvalid()
        {
            // Arrange
            var password = "my_secure_password";
            var hashedPassword = _passwordEncoder.Encode(password);
            var invalidHashedPassword = "invalid_hashed_password"; // An invalid hash (not following BCrypt format)

            // Act
            var result = _passwordEncoder.Verify(invalidHashedPassword, hashedPassword);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
