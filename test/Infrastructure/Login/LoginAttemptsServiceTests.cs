using System;
using NUnit.Framework;
using App.Security;

namespace App.Security.Tests
{
    [TestFixture]
    public class LoginAttemptsServiceTests
    {
        private ILoginAttemptsService _loginAttemptsService;

        [SetUp]
        public void Setup()
        {
            _loginAttemptsService = new LoginAttemptsService();
        }

        /// <summary>
        /// Tests that a user can register a failed login attempt and 
        /// that the number of remaining attempts decreases.
        /// </summary>
        [Test]
        public void RegisterFailedAttempt_ShouldIncrementFailedAttempts()
        {
            // Arrange
            var usernameId = "user123";

            // Act
            _loginAttemptsService.RegisterFailedAttempt(usernameId);
            _loginAttemptsService.RegisterFailedAttempt(usernameId);

            // Assert
            Assert.That(_loginAttemptsService.GetRemainingAttempts(usernameId), Is.EqualTo(3), 
                "There should be 3 remaining attempts after 2 failed attempts.");
        }

        /// <summary>
        /// Tests that after a user exceeds the maximum number of 
        /// failed attempts, they are locked out.
        /// </summary>
        [Test]
        public void RegisterFailedAttempt_ShouldLockUserAfterMaxAttempts()
        {
            // Arrange
            var usernameId = "user123";

            // Act
            for (int i = 0; i < 5; i++)
            {
                _loginAttemptsService.RegisterFailedAttempt(usernameId);
            }

            // Assert
            Assert.That(_loginAttemptsService.IsUserLockedOut(usernameId), Is.True, 
                "User should be locked out after 5 failed attempts.");
        }

        /// <summary>
        /// Tests that a user who has been locked out can be unlocked 
        /// after the lockout duration has expired.
        /// </summary>
        [Test]
        public void IsUserLockedOut_ShouldUnlockUserAfterDuration()
        {
            // Arrange
            var usernameId = "user123";
            for (int i = 0; i < 5; i++)
            {
                _loginAttemptsService.RegisterFailedAttempt(usernameId);
            }

            // Simulate waiting for the lockout duration to expire
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1.5)); // Wait for 1.5 minutes

            // Act & Assert
            Assert.That(_loginAttemptsService.IsUserLockedOut(usernameId), Is.True, 
                "User should not be locked out after the lockout duration has expired.");
        }

        /// <summary>
        /// Tests that remaining attempts return to the maximum after a 
        /// user successfully logs in.
        /// </summary>
        [Test]
        public void ResetLoginAttempts_ShouldResetAttemptsAfterSuccessfulLogin()
        {
            // Arrange
            var usernameId = "user123";
            for (int i = 0; i < 3; i++)
            {
                _loginAttemptsService.RegisterFailedAttempt(usernameId);
            }

            // Act
            _loginAttemptsService.ResetLoginAttempts(usernameId);

            // Assert
            Assert.That(_loginAttemptsService.GetRemainingAttempts(usernameId), Is.EqualTo(5), 
                "Remaining attempts should be reset to maximum after successful login.");
            Assert.That(_loginAttemptsService.IsUserLockedOut(usernameId), Is.False, 
                "User should not be locked out after reset of login attempts.");
        }

        /// <summary>
        /// Tests that when there are no previous attempts, 
        /// the user is not locked out and has maximum attempts remaining.
        /// </summary>
        [Test]
        public void GetRemainingAttempts_ShouldReturnMaxAttemptsWhenNoPreviousAttempts()
        {
            // Arrange
            var usernameId = "user123";

            // Act
            var remainingAttempts = _loginAttemptsService.GetRemainingAttempts(usernameId);

            // Assert
            Assert.That(remainingAttempts, Is.EqualTo(5), 
                "There should be 5 remaining attempts when no previous attempts have been made.");
            Assert.That(_loginAttemptsService.IsUserLockedOut(usernameId), Is.False, 
                "User should not be locked out when no previous attempts have been made.");
        }
    }
}
