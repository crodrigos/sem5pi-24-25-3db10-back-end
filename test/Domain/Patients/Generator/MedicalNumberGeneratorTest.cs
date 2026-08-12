using Moq;
using App.Onion.Domain.Interfaces.PatientRepository;
using App.Onion.Infrastructure.Persistence;
using NUnit.Framework;
using System.Threading.Tasks;

namespace App.Onion.Tests
{
    /// <summary>
    /// Unit tests for the MedicalRecordNumberGeneratorImpl class, which generates medical record numbers (MRN).
    /// </summary>
    [TestFixture]
    public class MedicalRecordNumberGeneratorImplTests
    {
        /// <summary>
        /// Mock object for IPatientRepository to simulate database operations.
        /// </summary>
        private Mock<IPatientRepository> _patientRepositoryMock;

        /// <summary>
        /// Instance of MedicalRecordNumberGeneratorImpl to be tested.
        /// </summary>
        private MedicalRecordNumberGenerator _generator;

        /// <summary>
        /// Sets up the test environment by initializing mocks and the MRN generator.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _generator = new MedicalRecordNumberGenerator(_patientRepositoryMock.Object);
        }

        /// <summary>
        /// Tests if GenerateMRN generates the correct MRN when there are existing patients in the system.
        /// </summary>
        [Test]
        public async Task GenerateMRN_ShouldGenerateCorrectMRN_WhenPatientsExist()
        {
            // Arrange: Simulate that there are 5 existing patients.
            _patientRepositoryMock.Setup(repo => repo.GetPatientSize()).ReturnsAsync(5); 
            var expectedYear = DateTime.Now.ToString("yyyy");
            var expectedMonth = DateTime.Now.ToString("MM");
            var expectedSequential = "000006"; // The next sequential number is 6
            var expectedMRN = $"{expectedYear}{expectedMonth}{expectedSequential}";

            // Act: Call GenerateMRN to generate the medical record number.
            var result = await _generator.GenerateMedicalRecordNumber();

            // Assert: Verify that the generated MRN is correct.
            Assert.That(result, Is.EqualTo(MedicalRecordNumber.Create(expectedMRN)));
        }

        /// <summary>
        /// Tests if GenerateMRN generates the correct MRN when there are no existing patients in the system.
        /// </summary>
        [Test]
        public async Task GenerateMRN_ShouldGenerateCorrectMRN_WhenNoPatientsExist()
        {
            // Arrange: Simulate that there are no existing patients.
            _patientRepositoryMock.Setup(repo => repo.GetPatientSize()).ReturnsAsync(0); 
            var expectedYear = DateTime.Now.ToString("yyyy");
            var expectedMonth = DateTime.Now.ToString("MM");
            var expectedSequential = "000001"; // The first sequential number is 1
            var expectedMRN = $"{expectedYear}{expectedMonth}{expectedSequential}";

            // Act: Call GenerateMRN to generate the medical record number.
            var result = await _generator.GenerateMedicalRecordNumber();

            // Assert: Verify that the generated MRN is correct.
            Assert.That(result, Is.EqualTo(MedicalRecordNumber.Create(expectedMRN)));
        }
        
        
        /// <summary>
        /// Tests if GenerateMRN generates a unique MRN that does not already exist in the database.
        /// </summary>
        [Test]
        public async Task GenerateMRN_ShouldGenerateUniqueMRN_WhenPatientsExist()
        {
            // Arrange: Simulate that there are existing patients in the database.
            // Let's say we have a patient with MRN "20231000001"
            var existingMRN = "20241000001";

            _patientRepositoryMock.Setup(repo => repo.GetPatientSize()).ReturnsAsync(1);

            var now = DateTime.Now;
            var expectedYear = now.ToString("yyyy");
            var expectedMonth = now.ToString("MM");
            var expectedSequential = "000001"; 
            var expectedMRN = $"{expectedYear}{expectedMonth}{expectedSequential}";

            // Act
            var result = await _generator.GenerateMedicalRecordNumber();

            Assert.That(result.Value, Is.Not.EqualTo(MedicalRecordNumber.Create(expectedMRN).Value)); // Ensure it matches the expected format
        }
    }
}
