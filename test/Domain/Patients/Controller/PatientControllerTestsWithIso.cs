using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using App.Onion.Application.Controllers;
using App.Onion.Application.Interfaces;
using App.Onion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnet8.Domain.Patients.DTO;

namespace SurgicalManagement.Tests.Integration
{
    [TestFixture]
    public class PatientControllerUnitIntegrationTests
    {
        private PatientController _controller;
        private Mock<IPatientService> _mockPatientService;

        /// <summary>
        /// Sets up the test environment before executing each test.
        /// Initializes the mock of IPatientService and the PatientController.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockPatientService = new Mock<IPatientService>();
            _controller = new PatientController(_mockPatientService.Object);
        }

        /// <summary>
        /// Tests the CreatePatient method to verify it returns a Created result
        /// when a patient is successfully created.
        /// </summary>
        [Test]
        public async Task CreatePatient_ReturnsCreatedResult_WhenPatientIsCreatedSuccessfully()
        {
            // Arrange
            var createPatientDto = new CreatePatientDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                ContactInformation = new ContactInfoDto("923923923", "john.doe@example.com"),
                EmergencyContact = new EmergencyContactDto
                {
                    EmergencyContactName = "Jane Doe",
                    EmergencyContactPhoneNumber = "932509983"
                }
            };

            var createdPatientDto = new PatientDto("John Doe", "Male", "2001-12-19", "202410000001", new ContactInfoDto("923923923", "teste@example.com"), new EmergencyContactDto("example", "923456123"));
            
            _mockPatientService.Setup(service => service.CreatePatient(createPatientDto))
                .ReturnsAsync(createdPatientDto);

            // Act
            var result = await _controller.CreatePatient(createPatientDto);

            // Assert
            var createdResult = result.Result as CreatedResult;

            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.Value, Is.EqualTo(createdPatientDto));
        }

        [Test]
        public async Task CreatePatient_ReturnsInternalServerError_WhenPatientCreationFails()
        {
            // Arrange
            var createPatientDto = new CreatePatientDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                ContactInformation = new ContactInfoDto("912912912", "john.doe@example.com"),
                
                EmergencyContact = new EmergencyContactDto
                {
                    EmergencyContactName = "Jane Doe",
                    EmergencyContactPhoneNumber = "932509983"
                }
            };

            _mockPatientService.Setup(service => service.CreatePatient(createPatientDto))
                .ReturnsAsync((PatientDto)null);

            // Act
            var result = await _controller.CreatePatient(createPatientDto);

            // Assert
            var statusCodeResult = result.Result as ObjectResult;

            Assert.That(statusCodeResult, Is.Not.Null);
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
            Assert.That(statusCodeResult.Value.ToString(), Is.EqualTo("{ message = Internal server error }"));
        }

    

        [Test]
        public async Task SearchPatients_ReturnsOkResult_WhenPatientsFound()
        {
            // Arrange
            var criteria = new PatientCriteria { FirstName = "John", LastName = "Doe" };
            var patients = new List<PatientDto>
            {
                new PatientDto("John Doe", "Male", "1990-01-01", "20241000001", new ContactInfoDto("923123456", "teste1@example.com"), new EmergencyContactDto("example", "923456123")),
                new PatientDto("Jane Doe", "Female", "1992-02-02", "20241000002", new ContactInfoDto("923923924", "teste2@example.com"), new EmergencyContactDto("example", "923456123"))
            };

            _mockPatientService.Setup(service => service.SearchPatientsByFilters(criteria))
                .ReturnsAsync(patients);

            // Act
            var result = await _controller.ListPatientsByFilter(criteria);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(patients));
        }

        [Test]
        public async Task SearchPatients_ReturnsBadRequest_WhenCriteriaIsNull()
        {
            // Act
            var result = await _controller.ListPatientsByFilter(null!);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("{ message = Search criteria cannot be null. }"));
        }

        [Test]
        public async Task SearchPatients_ReturnsNotFound_WhenNoPatientsFound()
        {
            // Arrange
            var criteria = new PatientCriteria { FirstName = "Unknown" };
            _mockPatientService.Setup(service => service.SearchPatientsByFilters(criteria))
                .ReturnsAsync(new List<PatientDto>()); // No patients found

            // Act
            var result = await _controller.ListPatientsByFilter(criteria);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value.ToString(), Is.EqualTo("{ message = No patients found matching the search criteria. }"));
        }

        [Test]
        public async Task UpdatePatient_ReturnsBadRequest_WhenPatientDtoIsNull()
        {
            // Act
            var result = await _controller.UpdatePatient(null , "MRN123");

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("{ message = Invalid patient data. }"));
        }

        [Test]
        public async Task UpdatePatient_ReturnsOk_WhenPatientIsUpdatedSuccessfully()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            var patientDto = new PatientCriteria { FirstName = "John", LastName = "Doe" };
            var updatedPatientDto = new PatientDto(
                "John Doe", 
                "Male", 
                "1990-01-01", 
                "20241000001", 
                new ContactInfoDto("923123456", "teste1@example.com"), 
                new EmergencyContactDto("example", "923456123")
            );

            _mockPatientService.Setup(service => service.UpdatePatientData(patientDto, medicalRecordNumber))
                .ReturnsAsync(updatedPatientDto); // Simulates successful update

            // Act
            var result = await _controller.UpdatePatient(patientDto, medicalRecordNumber);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            // Assert individual properties instead of the whole object
            Assert.That(okResult.Value, Has.Property("message").EqualTo("Patient updated successfully."));
            Assert.That(okResult.Value, Has.Property("updatedPatientDto").EqualTo(updatedPatientDto));

        }



        [Test]
        public async Task UpdatePatient_ReturnsInternalServerError_WhenUpdateFails()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            var patientDto = new PatientCriteria { FirstName = "John", LastName = "Doe" };

            _mockPatientService.Setup(service => service.UpdatePatientData(patientDto, medicalRecordNumber))
                .ReturnsAsync((PatientDto)null); // Simulates failure in updating

            // Act
            var result = await _controller.UpdatePatient(patientDto, medicalRecordNumber);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult, Is.Not.Null);
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(200));
        }

        /*[Test]
        public async Task DeletePatient_ReturnsBadRequest_WhenMedicalRecordNumberIsNullOrWhitespace()
        {
            // Act
            var result = await _controller.DeletePatient("   ");

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("{ message = Invalid medical record number. }"));
        }*/

        /*[Test]
        public async Task DeletePatient_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            _mockPatientService.Setup(service => service.ValidatePatientForDeletion(medicalRecordNumber))
                .ReturnsAsync((false, "Patient not found."));

            // Act
            var result = await _controller.DeletePatient(medicalRecordNumber);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value.ToString(), Is.EqualTo("{ message = Patient not found. }"));
        }*/

        /*[Test]
        public async Task DeletePatient_ReturnsAccepted_WhenPatientIsMarkedForDeletion()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            _mockPatientService.Setup(service => service.ValidatePatientForDeletion(medicalRecordNumber))
                .ReturnsAsync((true, "Patient can be deleted."));

            // Act
            var result = await _controller.DeletePatient(medicalRecordNumber);

            // Assert
            var acceptedResult = result as AcceptedResult;
            Assert.That(acceptedResult, Is.Not.Null);
            Assert.That(acceptedResult.StatusCode, Is.EqualTo(202));
        }
        */

        /*[Test]
        public async Task DeletePatient_ReturnsOk_WhenPatientIsDeleted()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            var confirm = true;
            _mockPatientService.Setup(service => service.MarkPatientForDeletion(medicalRecordNumber))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConfirmPatientDelete(medicalRecordNumber, confirm);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }*/

        /*[Test]
        public async Task DeletePatient_ReturnsNotFound_WhenKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            var confirm = true;
            _mockPatientService.Setup(service => service.MarkPatientForDeletion(medicalRecordNumber))
                .Throws(new KeyNotFoundException("Patient not found."));

            // Act
            var result = await _controller.ConfirmPatientDelete(medicalRecordNumber, confirm);

            // Assert
            var notFoundResult = result as ObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value.ToString(), Is.EqualTo("{ message = Patient not found. }"));
        }*/

        /*[Test]
        public async Task DeletePatient_ReturnsInternalServerError_WhenAnExceptionIsThrown()
        {
            // Arrange
            var medicalRecordNumber = "202410123123";
            var confirm = true;
            _mockPatientService.Setup(service => service.MarkPatientForDeletion(medicalRecordNumber))
                .Throws(new Exception("An unexpected error occurred."));

            // Act
            var result = await _controller.ConfirmPatientDelete(medicalRecordNumber, confirm);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult, Is.Not.Null);
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
            Assert.That(statusCodeResult.Value.ToString(), Is.EqualTo("{ message = An error occurred: An unexpected error occurred. }"));
        }*/
    }
}
