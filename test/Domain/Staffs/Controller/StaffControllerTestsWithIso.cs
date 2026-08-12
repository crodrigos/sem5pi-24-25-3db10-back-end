using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using dddnet8.Controllers;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Specializations.DTO;
using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.Interfaces;

namespace dddnet8.Tests
{
    [TestFixture]
    public class StaffControllerTests
    {
        private StaffController _controller;
        private Mock<IStaffService> _mockStaffService;
        private Mock<ILogger<StaffController>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockStaffService = new Mock<IStaffService>();
            _mockLogger = new Mock<ILogger<StaffController>>();
            _controller = new StaffController(_mockStaffService.Object, _mockLogger.Object);
        }

        /*[Test]
        public async Task CreateStaff_ReturnsCreatedResult_WhenStaffIsCreatedSuccessfully()
        {
            // Arrange
            var createStaffDto = new CreateStaffDto("John", "Doe", new SpecializationDto("Cardiology", ""),
                new ContactInfoDto("932932932", "john.doe@example.com"), "Doctor");
            var staffDto = new StaffDto("John Doe", "Cardiology", "D1234",
                new ContactInfoDto("923923923", "john.doe@example.com"));
            _mockStaffService.Setup(service => service.CreateStaffAsync(createStaffDto))
                .ReturnsAsync(staffDto);

            // Act
            var result = await _controller.CreateStaff(createStaffDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.Value, Is.EqualTo(staffDto));
            Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.CreateStaff)));
        }
        */


        [Test]
        public async Task CreateStaff_ReturnsBadRequest_WhenCreateStaffDtoIsNull()
        {
            // Act
            var result = await _controller.CreateStaff(null);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("Staff data must not be null."));
        }

        [Test]
        public async Task SearchStaffByCriteria_ReturnsOk_WhenStaffFound()
        {
            // Arrange
            var criteria = new StaffCriteria { FirstName = "John" };
            var staffList = new List<StaffDto>
            {
                new StaffDto("John Doe", "Cardiology", "D1234",
                    new ContactInfoDto("923923923", "john.doe@example.com"))
            };

            _mockStaffService.Setup(service => service.SearchStaffByFiltersAsync(criteria))
                .ReturnsAsync(staffList);

            // Act
            var result = await _controller.SearchStaffByCriteria(criteria);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(staffList));
        }

        [Test]
        public async Task SearchStaffByCriteria_ReturnsNotFound_WhenNoStaffFound()
        {
            // Arrange
            var criteria = new StaffCriteria { FirstName = "Unknown" };

            _mockStaffService.Setup(service => service.SearchStaffByFiltersAsync(criteria))
                .ReturnsAsync(new List<StaffDto>());

            // Act
            var result = await _controller.SearchStaffByCriteria(criteria);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value.ToString(), Is.EqualTo("{ message = No staff members found matching the search criteria. }"));
        }

        [Test]
        public async Task UpdateStaff_ReturnsOk_WhenStaffIsUpdatedSuccessfully()
        {
            // Arrange
            var licenseNumber = "LIC12345";
            var updateCriteria = new StaffCriteria { FirstName = "Jane" };
            var updatedStaffDto = new StaffDto("John Doe", "Cardiology", "D1234", 
                new ContactInfoDto("923923923", "john.doe@example.com"));


            _mockStaffService.Setup(service => service.UpdateStaffData(updateCriteria, licenseNumber))
                .ReturnsAsync(updatedStaffDto);

            // Act
            var result = await _controller.UpdateStaff(licenseNumber, updateCriteria);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value.ToString(), Contains.Substring("Staff updated successfully"));
        }

        [Test]
        public async Task UpdateStaff_ReturnsNotFound_WhenStaffDoesNotExist()
        {
            // Arrange
            var licenseNumber = "NONEXISTENT";
            var updateCriteria = new StaffCriteria { FirstName = "Jane" };

            _mockStaffService.Setup(service => service.UpdateStaffData(updateCriteria, licenseNumber))
                .Throws(new KeyNotFoundException("Staff not found."));

            // Act
            var result = await _controller.UpdateStaff(licenseNumber, updateCriteria);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value.ToString(), Is.EqualTo("{ message = Staff not found. }"));
        }

        [Test]
        public async Task DeleteStaff_ReturnsAccepted_WhenStaffCanBeDeleted()
        {
            // Arrange
            var licenseNumber = "LIC12345";

            _mockStaffService.Setup(service => service.ValidateStaffForDeletion(licenseNumber))
                .ReturnsAsync((true, "Staff can be deleted."));

            // Act
            var result = await _controller.DeleteStaff(licenseNumber);

            // Assert
            var acceptedResult = result as AcceptedResult;
            Assert.That(acceptedResult, Is.Not.Null);
            Assert.That(acceptedResult.StatusCode, Is.EqualTo(202));
        }

        [Test]
        public async Task ConfirmStaffDelete_ReturnsOk_WhenStaffDeletionConfirmed()
        {
            // Arrange
            var licenseNumber = "L1234";
            var confirm = true;

            _mockStaffService.Setup(service => service.MarkStaffForDeletion(licenseNumber))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConfirmStaffDelete(licenseNumber);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value.ToString(), Contains.Substring("Staff marked for deletion successfully."));
        }

        [Test]
        public async Task ConfirmStaffDelete_ReturnsNotFound_WhenStaffNotFoundForDeletion()
        {
            // Arrange
            var licenseNumber = "LIC_NOTFOUND";
            var confirm = true;

            _mockStaffService.Setup(service => service.MarkStaffForDeletion(licenseNumber))
                .Throws(new KeyNotFoundException("Staff not found."));

            // Act
            var result = await _controller.ConfirmStaffDelete(licenseNumber);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value.ToString(), Is.EqualTo("{ message = Staff not found. }"));
        }
    }
}
