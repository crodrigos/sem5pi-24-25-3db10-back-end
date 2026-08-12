using App.Passsword.Encoder;
using App.PassswordPolicy;
using App.Password.Generator;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Authentication.token;
using dddnet8.Domain.BackOfficeEmail;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using Moq;
using NUnit.Framework;
using SurgicalManagement.Domain.Domain;

namespace YourNamespace.Tests;

/// <summary>
///     Test class for SystemUserService.
///     This class contains unit tests to verify the behavior of the methods in SystemUserService,
///     specifically the user creation and retrieval functionalities.
/// </summary>
[TestFixture]
public class SystemUserServiceTests
{
    /// <summary>
    ///     Sets up the test environment before each test case runs.
    ///     It initializes mocks for the dependencies and creates an instance of SystemUserService.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<ISystemUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _passwordEncoderMock = new Mock<IPasswordEncoder>();
        _passwordPolicyMock = new Mock<IPasswordPolicy>();
        _passwordGeneratorMock = new Mock<IPasswordGenerator>();
        _officeEmailGeneratorMock = new Mock<IBackOfficeEmailGenerator>();
        _staffRepositoryMock = new Mock<IStaffRepository>();
       

        _userService = new SystemUserService(
            _userRepositoryMock.Object,
            _emailServiceMock.Object,
            _tokenServiceMock.Object,
            _passwordEncoderMock.Object,
            _passwordPolicyMock.Object,
            _passwordGeneratorMock.Object,
            _officeEmailGeneratorMock.Object,
            _staffRepositoryMock.Object
        );
    }

    private SystemUserService _userService;
    private Mock<ISystemUserRepository> _userRepositoryMock;
    private Mock<IEmailService> _emailServiceMock;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<IPasswordEncoder> _passwordEncoderMock;
    private Mock<IPasswordPolicy> _passwordPolicyMock;
    private Mock<IPasswordGenerator> _passwordGeneratorMock;
    private Mock<IBackOfficeEmailGenerator> _officeEmailGeneratorMock;
    private Mock<IStaffRepository> _staffRepositoryMock;
    


    /// <summary>
    ///     Tests that GetUserByEmail returns the user if it exists.
    /// </summary>
    [Test]
    public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userEmail = "test@example.com";
        var user = new SystemUser(EmailAddress.Create(userEmail), EmailAddress.Create(userEmail), UserRole.Admin,
            "password");

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(EmailAddress.Create(userEmail))).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByEmail(userEmail);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.EmailAddress, Is.EqualTo(EmailAddress.Create(userEmail)));
    }

    /// <summary>
    ///     Tests that GetUserByEmail throws an exception when the user does not exist.
    /// </summary>
    [Test]
    public async Task GetUserByEmail_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
    {
        // Arrange
        var userEmail = "nonexistent@example.com";
        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(EmailAddress.Create(userEmail)))
            .ReturnsAsync((SystemUser)null);

        // Act & Assert
        Assert.That(async () => await _userService.GetUserByEmail(userEmail),
            Throws.InvalidOperationException.With.Message.EqualTo("User does not exist."));
    }

    /// <summary>
    ///     Tests that ActivateUserAccount activates the user and resets the password.
    /// </summary>
    [Test]
    public async Task ActivateUserAccount_ShouldActivateUserAndResetPassword()
    {
        // Arrange
        var userEmail = "test@example.com";
        var password = "ValidPassword123!";
        var user = new SystemUser(EmailAddress.Create(userEmail), EmailAddress.Create(userEmail), UserRole.Admin,
            "oldP@ssword2933");

        // Mocking repository and password policy
        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(EmailAddress.Create(userEmail))).ReturnsAsync(user);
        _passwordPolicyMock.Setup(policy => policy.isSatisfiedBy(password)).Returns(true);
        _passwordEncoderMock.Setup(encoder => encoder.Encode(password))
            .Returns("hashedPassword"); // Ensure this returns a valid string
        _userRepositoryMock.Setup(repo => repo.ActivateUserAccountAsync(It.IsAny<SystemUser>()))
            .Returns(Task.CompletedTask); // Ensure the activation method does not throw exceptions

        // Act
        await _userService.ActivateUserAccount(userEmail, password);

        // Assert
        Assert.That(user.IsActive, Is.True); // User should be activated
        _userRepositoryMock.Verify(repo => repo.ActivateUserAccountAsync(user),
            Times.Once); // Verify activation method was called once
    }


    /// <summary>
    ///     Tests that ActivateUserAccount throws an exception when the password policy is not satisfied.
    /// </summary>
    [Test]
    public async Task ActivateUserAccount_ShouldThrowPasswordException_WhenPasswordPolicyNotSatisfied()
    {
        // Arrange
        var userEmail = "test@example.com";
        var password = "short";
        _passwordPolicyMock.Setup(policy => policy.isSatisfiedBy(password)).Returns(false);

        // Act & Assert
        Assert.That(async () => await _userService.ActivateUserAccount(userEmail, password),
            Throws.Exception.TypeOf<PasswordException>().With.Message
                .EqualTo("A senha não corresponde à política de senha."));
    }

    /// <summary>
    ///     Tests that ResetUserPassword updates the user's password and saves the changes.
    /// </summary>
    [Test]
    public async Task ResetUserPassword_ShouldUpdatePasswordAndSaveChanges()
    {
        // Arrange
        var user = new SystemUser(EmailAddress.Create("test@example.com"), EmailAddress.Create("test@example.com"),
            UserRole.Admin, "old_password");
        var newPassword = "NewPassword123!";

        _passwordEncoderMock.Setup(encoder => encoder.Encode(newPassword)).Returns("hashed_password");

        // Act
        await _userService.ResetUserPassword(user, newPassword);

        // Assert
        Assert.That(user.Password, Is.EqualTo("hashed_password"));
        _userRepositoryMock.Verify(repo => repo.ActivateUserAccountAsync(user), Times.Once);
    }
    
    /// <summary>
        ///     Tests that DeleteUser returns success when the user is deleted successfully.
        /// </summary>
        [Test]
        public async Task DeleteUser_ShouldReturnSuccess_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            var username = "test@example.com";
            var user = new SystemUser(EmailAddress.Create("test2@example.com"), EmailAddress.Create("test@example.com"),
                UserRole.Patient, "old_password");

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(EmailAddress.Create(username))).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.RemoveUserAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.DeleteUser(username);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo("User deleted successfully."));
            _userRepositoryMock.Verify(repo => repo.RemoveUserAsync(user), Times.Once);
        }

        /// <summary>
        ///     Tests that DeleteUser returns failure when the username is null or empty.
        /// </summary>
        [Test]
        public async Task DeleteUser_ShouldReturnFailure_WhenUsernameIsNullOrEmpty()
        {
            // Arrange
            var username = "";

            // Act
            var result = await _userService.DeleteUser(username);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username cannot be null or empty."));
            _userRepositoryMock.Verify(repo => repo.RemoveUserAsync(It.IsAny<SystemUser>()), Times.Never);
        }

        /// <summary>
        ///     Tests that DeleteUser returns failure when the user is not found.
        /// </summary>
        [Test]
        public async Task DeleteUser_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistent@example.com";

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(EmailAddress.Create(username)))
                .ReturnsAsync((SystemUser)null);

            // Act
            var result = await _userService.DeleteUser(username);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found."));
            _userRepositoryMock.Verify(repo => repo.RemoveUserAsync(It.IsAny<SystemUser>()), Times.Never);
        }

        /// <summary>
        ///     Tests that DeleteUser returns failure with an error message when an unexpected error occurs.
        /// </summary>
        [Test]
        public async Task DeleteUser_ShouldReturnFailure_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var username = "test@example.com";
            var user = new SystemUser(EmailAddress.Create("test1@example.com"), EmailAddress.Create("test@example.com"),
                UserRole.Patient, "old_password");

            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(EmailAddress.Create(username)))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.RemoveUserAsync(user))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = await _userService.DeleteUser(username);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Does.StartWith("An unexpected error occurred"));
            _userRepositoryMock.Verify(repo => repo.RemoveUserAsync(user), Times.Once);
        }
}