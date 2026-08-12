using System.Threading.Tasks;
using App.Domain.SystemUser;
using App.Login;
using App.Security;
using App.SystemUser.Domain.DTO;
using dddnet8.Domain.Authentication;
using dddnet8.Domain.Authentication.token;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace App.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private Mock<IAuthService> _mockAuthService;
        private Mock<ITokenService> _mockTokenService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the mock auth service and the controller before each test
            _mockAuthService = new Mock<IAuthService>();
            _mockTokenService = new Mock<ITokenService>();
            _authController = new AuthController(_mockAuthService.Object, _mockTokenService.Object);
        }

        /// <summary>
        /// Test that the Login method returns BadRequest when the LoginDto is null.
        /// </summary>
        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenLoginDtoIsNull()
        {
            // Arrange
            LoginDto loginDto = null;

            // Act
            var result = await _authController.Login(loginDto); 
            
            // Assert
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Invalid login data"));
        }

        /// <summary>
        /// Test that the Login method returns Ok when the login is successful.
        /// </summary>
        [Test]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginDto = new LoginDto();
            var token = "mockToken";
            var userDto = new SystemUserDto("testUser", "test@example.com", "Admin");

            _mockAuthService.Setup(x => x.Login(loginDto))
                .ReturnsAsync((token, "Login successful", userDto)); // Include User data in the return

            // Act
            var result = await _authController.Login(loginDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            // Verifying the full structure using an anonymous object
            // Assert individual properties instead of the whole object
            Assert.That(okResult.Value, Has.Property("Token").EqualTo(token));
            Assert.That(okResult.Value, Has.Property("Message").EqualTo("Login successful"));
            Assert.That(okResult.Value, Has.Property("User").EqualTo(userDto));

        }


        /// <summary>
        /// Test that the ActivateAccount method returns BadRequest when passwords do not match.
        /// </summary>
        [Test]
        public async Task ActivateAccount_ShouldReturnBadRequest_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var request = new PasswordDto { Password = "pass1", Confirmation = "pass2" };

            var token = "teste";

            // Act
            var result = await _authController.ActivateAccount(token,request);

            // Assert
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Password and confirmation do not match."));
        }

        /// <summary>
        /// Test that the ActivateAccount method returns Ok when the account activation is successful.
        /// </summary>
        [Test]
        public async Task ActivateAccount_ShouldReturnOk_WhenActivationIsSuccessful()
        {
            // Arrange
            var request = new PasswordDto { Password = "pass1", Confirmation = "pass1" };
            var token = "test";
            _mockAuthService.Setup(x => x.ActivateUserAccount(request,token)).ReturnsAsync((true, "Account activated successfully"));

            // Act
            var result = await _authController.ActivateAccount(token,request);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.That(okResult?.Value?.ToString(), Is.EqualTo("Account activated successfully"));
        }

        /// <summary>
        /// Test that the ActivateAccount method returns InternalServerError when activation fails.
        /// </summary>
        [Test]
        public async Task ActivateAccount_ShouldReturnInternalServerError_WhenActivationFails()
        {
            // Arrange
            var request = new PasswordDto { Password = "pass1", Confirmation = "pass1" };
            var token = "test";
            _mockAuthService.Setup(x => x.ActivateUserAccount(request,token)).ReturnsAsync((false, "Activation failed"));

            // Act
            var result = await _authController.ActivateAccount(token,request);
            
            // Assert
            var internalServerErrorResult = result as ObjectResult;
            Assert.That(internalServerErrorResult, Is.Not.Null);
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(500));
            Assert.That(internalServerErrorResult.Value, Is.EqualTo("Failed to activate account.Activation failed"));
        }
    }
}
