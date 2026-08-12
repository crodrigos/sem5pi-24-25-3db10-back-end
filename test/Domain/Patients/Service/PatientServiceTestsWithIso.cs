using Moq;
using NUnit.Framework;
using App.Onion.Application.Dtos;
using App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;
using App.Onion.Domain.Interfaces.PatientRepository;
using App.Onion.Domain.V.O.Patient;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Patients.DataModel;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;


namespace App.Onion.Tests
{
    [TestFixture]
    public class PatientServiceImplTests
    {
        private Mock<IPatientRepository> _patientRepositoryMock;
        private Mock<IMedicalRecordNumberGenerator> _medicalRecordNumberGeneratorMock;
        private Mock<ILogService<Patient>> _patientLogServiceMock;
        private Mock<IEmailService> _emailServiceMock;
        private PatientService _patientService;
        private Mock<ISystemUserService> _systemUserServiceMock;

        [SetUp]
        public void SetUp()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _medicalRecordNumberGeneratorMock = new Mock<IMedicalRecordNumberGenerator>();
            _emailServiceMock = new Mock<IEmailService>();
            _patientLogServiceMock = new Mock<ILogService<Patient>>();
            _systemUserServiceMock = new Mock<ISystemUserService>();

            _patientService = new PatientService(_patientRepositoryMock.Object, _medicalRecordNumberGeneratorMock.Object, _systemUserServiceMock.Object,
                _emailServiceMock.Object, _patientLogServiceMock.Object);
        }

        /// <summary>
        /// Tests the creation of a patient when a valid DTO is provided.
        /// </summary>
        [Test]
        public async Task CreatePatient_ShouldAddPatient_WhenValidDtoProvided()
        {
            var patientDto = new CreatePatientDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                ContactInformation = new ContactInfoDto("923923923", "john.doe@example.com"),
                EmergencyContact = new EmergencyContactDto
                {
                    EmergencyContactName = "Ana B",
                    EmergencyContactPhoneNumber = "987654321"
                }
            };

            _medicalRecordNumberGeneratorMock.Setup(gen => gen.GenerateMedicalRecordNumber())
                .ReturnsAsync(MedicalRecordNumber.Create("202410123123"));

            _patientRepositoryMock.Setup(repo => repo.AddPatientAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

            var result = await _patientService.CreatePatient(patientDto);

            Assert.That(result, Is.Not.Null);
            _patientRepositoryMock.Verify(repo => repo.AddPatientAsync(It.IsAny<Patient>()), Times.Once);
        }

        /// <summary>
        /// Tests the updating of a patient when the patient already exists.
        /// </summary>
        [Test]
        public async Task UpdatePatient_ShouldUpdatePatient_WhenPatientExists()
        {
            // Arrange
            var patientCriteria = new PatientCriteria
            {
                FirstName = "Jane",
                LastName = "Doe",
                Gender = "Female",
                ContactInformation = new ContactInfoDto("999999999", "updated@example.com"),
            };

            var existingPatient = CreateTestPatient(); // Método para criar um paciente de teste
            var medicalRecordNumber = existingPatient.MedicalRecordNumber.ToString();
    
            var existingPatientDataModel = PatientMapper.ToDataModel(existingPatient);

            _patientRepositoryMock.Setup(repo => repo.GetPatientByMedicalRecordNumber(It.IsAny<MedicalRecordNumber>()))
                .ReturnsAsync(existingPatient);

            // Aqui, você pode usar o _patientRepositoryMock para verificar se o método de atualização foi chamado
            _patientRepositoryMock.Setup(repo => repo.UpdatePatientDataAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask); // Simular a atualização sem fazer nada

            // Act
            var result = await _patientService.UpdatePatientData(patientCriteria, medicalRecordNumber);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Gender, Is.EqualTo(patientCriteria.Gender));
    
            // Verifica se o método de atualização foi chamado uma vez
            _patientRepositoryMock.Verify(repo => repo.UpdatePatientDataAsync(It.IsAny<Patient>()), Times.Once);
        }


        /// <summary>
        /// Tests the search for patients when criteria are provided.
        /// </summary>
        [Test]
        public async Task SearchPatients_ShouldReturnPatients_WhenCriteriaProvided()
        {
            var criteria = new PatientCriteria { FullName = "Doe" };

            var patients = new List<PatientDataModel> { PatientMapper.ToDataModel(CreateTestPatient()), PatientMapper.ToDataModel(CreateTestPatient1()) };

            _patientRepositoryMock.Setup(repo => repo.SearchPatientsByFiltersAsync(criteria)).ReturnsAsync(patients.Select(PatientMapper.ToDomainModel));

            var result = await _patientService.SearchPatientsByFilters(criteria);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(p => p.FullName.Contains(criteria.FullName)), Is.True);
        }

        /// <summary>
        /// Tests the creation of a patient and verifies that an exception is thrown when the gender is invalid.
        /// </summary>
        [Test]
        public void CreatePatient_ShouldThrowException_WhenInvalidGender()
        {
            var patientDto = new CreatePatientDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = "Unknown",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _patientService.CreatePatient(patientDto));
            Assert.That(ex.Message, Is.EqualTo("Invalid gender"));
        }

        /// <summary>
        /// Helper method to create a test patient.
        /// </summary>
        private Patient CreateTestPatient()
        {
            var firstName = Name.Create("John");
            var lastName = Name.Create("Doe");
            var dob = DateOfBirth.Create(new DateTime(1990, 1, 1));
            var gender = Gender.Male;
            var medicalRecordNumber = MedicalRecordNumber.Create("202410123456");
            var contactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("alejandro@gmail.com"));
            var emergencyContact = EmergencyContact.Create("Jane Doe", "987654321");

            return new Patient(firstName, lastName, dob, gender, medicalRecordNumber, contactInfo, emergencyContact);
        }
        
        /// <summary>
        /// Helper method to create a second test patient.
        /// </summary>
        private Patient CreateTestPatient1()
        {
            var firstName = Name.Create("Jane");
            var lastName = Name.Create("Doe");
            var dob = DateOfBirth.Create(new DateTime(1990, 1, 1));
            var gender = Gender.Male;
            var medicalRecordNumber = MedicalRecordNumber.Create("202410456789");
            var contactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("teste@gmail.com"));
            var emergencyContact = EmergencyContact.Create("Jane Doe", "987654321");

            return new Patient(firstName, lastName, dob, gender, medicalRecordNumber, contactInfo, emergencyContact);
        }
    }
}
