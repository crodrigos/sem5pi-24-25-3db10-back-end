using App.Domain.SystemUser;
using App.SystemUserStuff;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using dddnet8.Domain.Authentication.token;
using YourNamespace.Controllers;

namespace YourNamespace.Tests
{
    /// <summary>
    /// Test class for UserController.
    /// This class contains unit tests to verify the behavior of the UserController methods, 
    /// particularly the CreateUser method.
    /// </summary>
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private Mock<ISystemUserService> _userServiceMock;
        private Mock<ITokenService> _tokenServiceMock;

        /// <summary>
        /// Sets up the test environment before each test case runs.
        /// It initializes the mock of ISystemUserService and creates an instance of UserController.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<ISystemUserService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userController = new UserController(_userServiceMock.Object, _tokenServiceMock.Object);
            
        }

        /// <summary>
        /// Tests that CreateUser returns a BadRequest result when the input userRequestDto is null.
        /// </summary>
        [Test]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUserRequestDtoIsNull()
        {
            // Act
            var result = await _userController.CreateUser(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid data."));
        }

        /// <summary>
        /// Tests that CreateUser returns a CreatedResult when a user is successfully created.
        /// </summary>
        [Test]
        public async Task CreateUser_ShouldReturnCreatedResult_WhenUserIsSuccessfullyCreated()
        {
            // Arrange
            var userRequestDto = new SystemUserRequestDto
            {
                EmailAddress = "test@example.com",
                Role = "Admin"
            };

            var createdUserDto = new SystemUserDto("test@example.com", "test@example.com", "Admin");

            _userServiceMock
                .Setup(service => service.CreateUser(userRequestDto))
                .ReturnsAsync(createdUserDto);

            // Act
            var result = await _userController.CreateUser(userRequestDto);

            // Assert
            var createdResult = result as CreatedResult;

            Assert.That(createdResult.Value, Is.Not.Null);
            Assert.That(createdResult.Value.ToString(), Is.EqualTo("Usuário criado com sucesso. Um e-mail de ativação foi enviado para test@example.com"));
        }

        /// <summary>
        /// Tests that CreateUser returns an InternalServerError result when an exception occurs during user creation.
        /// </summary>
        [Test]
        public async Task CreateUser_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var userRequestDto = new SystemUserRequestDto
            {
                EmailAddress = "test@example.com",
                Role = "Admin"
            };

            _userServiceMock
                .Setup(service => service.CreateUser(userRequestDto))
                .ThrowsAsync(new System.Exception("Unexpected error"));

            // Act
            var result = await _userController.CreateUser(userRequestDto);
            
            var createdResult = result as ObjectResult;

            // Assert
            Assert.That(createdResult, Is.InstanceOf<ObjectResult>());
            Assert.That(createdResult.StatusCode, Is.EqualTo(500)); // Expecting a 500 Internal Server Error
        }
        
        [Test]
        public async Task DeleteUser_ShouldReturnBadRequest_WhenUsernameIsNullOrEmpty()
        {
            // Arrange
            string username = null;

            // Act
            var result = await _userController.DeleteUser(username);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null), 
                Is.EqualTo("Username cannot be empty or null."));
        }


        [Test]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            string username = "nonexistentuser";
            _userServiceMock
                .Setup(service => service.DeleteUser(username))
                .ReturnsAsync((false, "User not found."));

            // Act
            var result = await _userController.DeleteUser(username);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null),
                Is.EqualTo("User not found."));
        }


        [Test]
        public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeletedSuccessfully()
        {
            // Arrange
            string username = "existinguser";
            _userServiceMock
                .Setup(service => service.DeleteUser(username))
                .ReturnsAsync((true, "User deleted successfully."));

            // Act
            var result = await _userController.DeleteUser(username);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null),
                Is.EqualTo("User deleted successfully."));
        }


        [Test]
        public async Task DeleteUser_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            string username = "erroruser";
            _userServiceMock
                .Setup(service => service.DeleteUser(username))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _userController.DeleteUser(username);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var internalServerErrorResult = result as ObjectResult;
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(500));

            var resultValue = internalServerErrorResult.Value;
            var messageValue = resultValue.GetType().GetProperty("message").GetValue(resultValue, null);
            var errorValue = resultValue.GetType().GetProperty("error").GetValue(resultValue, null);

            Assert.That(messageValue, Is.EqualTo("An unexpected error occurred. Please try again later."));
            Assert.That(errorValue, Is.EqualTo("Unexpected error"));
        }

    }
}
