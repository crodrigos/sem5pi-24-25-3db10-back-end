using NUnit.Framework;
using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;


namespace App.Domain.Tests
{
    [TestFixture]
    public class SystemUserTests
    {
        private global::dddnet8.Domain.SystemUsers.SystemUser _systemUser;
        private EmailAddress _username;
        private EmailAddress _emailAddress;
        private const string ValidPassword = "securepassword";
        private const string NewUsername = "newusername@example.com";

        [SetUp]
        public void SetUp()
        {
            _username = new EmailAddress("username@example.com");
            _emailAddress = new EmailAddress("user@example.com");
            _systemUser = new global::dddnet8.Domain.SystemUsers.SystemUser(_username, _emailAddress, UserRole.Admin, ValidPassword);
        }

        [Test]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Assert
            Assert.That(_systemUser.Username, Is.EqualTo(_username), "Username should be initialized correctly.");
            Assert.That(_systemUser.EmailAddress, Is.EqualTo(_emailAddress), "EmailAddress should be initialized correctly.");
            Assert.That(_systemUser.IsActive, Is.False, "New user should not be active by default.");
            Assert.That(_systemUser.Role, Is.EqualTo(UserRole.Admin), "Role should be initialized correctly.");
        }

        [Test]
        public void ChangeUsername_ShouldUpdateUsername_WhenValidUsernameIsProvided()
        {
            // Arrange
            var newUsername = new EmailAddress(NewUsername);

            // Act
            _systemUser.ChangeUsername(newUsername);

            // Assert
            Assert.That(_systemUser.Username, Is.EqualTo(newUsername), "Username should be updated correctly.");
        }

        [Test]
        public void ChangeUsername_ShouldThrowInvalidUsernameException_WhenUsernameIsNull()
        {
            // Act & Assert
            Assert.That(() => _systemUser.ChangeUsername(null),
                Throws.TypeOf<InvalidUsernameException>().With.Message.EqualTo("Username cannot be null or empty."));
        }

        [Test]
        public void ChangeUsername_ShouldThrowInvalidUsernameException_WhenUsernameIsEmpty()
        {
            // Act & Assert
            Assert.That(() => _systemUser.ChangeUsername(null),
                Throws.TypeOf<InvalidUsernameException>().With.Message.EqualTo("Username cannot be null or empty."));
        }

        [Test]
        public void ResetPassword_ShouldUpdatePassword_WhenValidPasswordIsProvided()
        {
            // Arrange
            var newPassword = "newsecurepassword";

            // Act
            _systemUser.ResetPassword(newPassword);

            // Assert
            Assert.That(_systemUser.Password, Is.EqualTo(newPassword), "Password should be updated correctly.");
        }

        [Test]
        public void ResetPassword_ShouldThrowPasswordException_WhenPasswordIsNull()
        {
            // Act & Assert
            Assert.That(() => _systemUser.ResetPassword(null),
                Throws.TypeOf<PasswordException>().With.Message.EqualTo("Password cannot be null or empty."));
        }

        [Test]
        public void ResetPassword_ShouldThrowPasswordException_WhenPasswordIsEmpty()
        {
            // Act & Assert
            Assert.That(() => _systemUser.ResetPassword(string.Empty),
                Throws.TypeOf<PasswordException>().With.Message.EqualTo("Password cannot be null or empty."));
        }

        [Test]
        public void ActivateAccount_ShouldSetIsActiveToTrue_WhenCalled()
        {
            // Act
            _systemUser.ActivateAccount();

            // Assert
            Assert.That(_systemUser.IsActive, Is.True, "Account should be active after activation.");
        }

        [Test]
        public void ActivateAccount_ShouldThrowAccountAlreadyActiveException_WhenCalledAgain()
        {
            // Arrange
            _systemUser.ActivateAccount(); // Activate first

            // Act & Assert
            Assert.That(() => _systemUser.ActivateAccount(),
                Throws.TypeOf<AccountAlreadyActiveException>().With.Message.EqualTo("Account is already active."));
        }

        [Test]
        public void DeactiveAccount_ShouldSetIsActiveToFalse_WhenCalled()
        {
            // Arrange
            _systemUser.ActivateAccount(); // First activate the account

            // Act
            _systemUser.DeactivateAccount();

            // Assert
            Assert.That(_systemUser.IsActive, Is.False, "Account should be inactive after deactivation.");
        }

        [Test]
        public void ChangeRole_ShouldUpdateUserRole_WhenCalled()
        {
            // Arrange
            var newRole = UserRole.Admin;

            // Act
            _systemUser.ChangeRole(newRole);

            // Assert
            Assert.That(_systemUser.Role, Is.EqualTo(newRole), "User role should be updated correctly.");
        }
    }
}
