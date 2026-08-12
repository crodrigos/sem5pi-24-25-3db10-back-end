using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.V.O;
using NUnit.Framework;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Specializations.DTO;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.SystemUsers;
using Name = dddnet8.Domain.Patients.V.O.Name;

namespace dddnet8.Tests.Domain.Staffs
{
    [TestFixture]
    public class StaffTests
    {
        private Staff _staff;

        /*[SetUp]
        public void Setup()
        {
            _staff = CreateTestStaff();
        }*/

        /*
        [Test]
        public void Constructor_ValidParameters_CreatesStaffSuccessfully()
        {
            // Arrange
            var firstName = Name.Create("John");
            var lastName = Name.Create("Doe");
            var specialization = new Specialization("Cardiology", "");
            var contactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("john@example.com"));
            var licenseNumber = new LicenseNumber("A1234");

            // Act
            var staff = new Staff(firstName, lastName, specialization, contactInfo, licenseNumber);

            // Assert
            Assert.That(staff.FirstName.ToString(), Is.EqualTo("John"));
            Assert.That(staff.LastName.ToString(), Is.EqualTo("Doe"));
            Assert.That(staff.FullName.ToString(), Is.EqualTo("John Doe"));
            Assert.That(staff.Specialization.Name, Is.EqualTo("Cardiology"));
            Assert.That(staff.ContactInfo.PhoneNumber.ToString(), Is.EqualTo("923211160"));
            Assert.That(staff.LicenseNumber.ToString(), Is.EqualTo("A1234"));
            Assert.That(staff.ContactInfo.EmailAddress.ToString(), Is.EqualTo("john@example.com"));
        }

        [Test]
        public void UpdateSpecialization_ValidSpecialization_UpdatesSuccessfully()
        {
            // Act
            _staff.UpdateSpecialization(new Specialization("Pediatrics", ""));

            // Assert
            Assert.That(_staff.Specialization.Name, Is.EqualTo("Pediatrics"));
            Assert.That(_staff.Specialization.Description, Is.EqualTo(""));
        }

        [Test]
        public void UpdateSpecialization_InvalidSpecialization_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _staff.UpdateSpecialization(new Specialization("", "")));
            Assert.That(ex.Message, Does.Contain("Name cannot be null, empty, or consist only of whitespace. (Parameter 'name')"));
        }

        [Test]
        public void UpdateStaff_UpdatesNameAndContactInfoSuccessfully()
        {
            // Arrange
            var updatedCriteria = new StaffCriteria
            {
                FirstName = "Jane",
                LastName = "Smith",
                ContactInformation = new ContactInfoDto("923211161", "jane@example.com"),
                Specialization = new SpecializationDto("Orthopedics", "")
            };

            // Act
            _staff.UpdateStaff(updatedCriteria);

            // Assert
            Assert.That(_staff.FirstName.ToString(), Is.EqualTo("Jane"));
            Assert.That(_staff.LastName.ToString(), Is.EqualTo("Smith"));
            Assert.That(_staff.FullName.ToString(), Is.EqualTo("Jane Smith"));
            Assert.That(_staff.ContactInfo.PhoneNumber.ToString(), Is.EqualTo("923211161"));
            Assert.That(_staff.ContactInfo.EmailAddress.ToString(), Is.EqualTo("jane@example.com"));
            Assert.That(_staff.Specialization.Name, Is.EqualTo("Orthopedics"));
            Assert.That(_staff.Specialization.Description, Is.EqualTo(""));
        }

        [Test]
        public void MarkForDeletion_MarksStaffForDeletion()
        {
            // Act
            _staff.MarkForDeletion();

            // Assert
            Assert.That(_staff.DeletionStatus.IsToDelete, Is.True);
            Assert.That(_staff.DeletionStatus.DeletionDate.HasValue, Is.True);
        }

        [Test]
        public void CanDelete_StaffMarkedForDeletion_ReturnsTrue()
        {
            // Arrange
            _staff.MarkForDeletion();

            // Act
            var canDelete = _staff.CanDelete();

            // Assert
            Assert.That(canDelete, Is.False);
        }

        [Test]
        public void HasSpecializationForOperationType_MatchingSpecialization_ReturnsTrue()
        {
            // Arrange
            var operationType = CreateTestOperationType(new Specialization(Name.Create("Cardiology"), Description.Create("result").Value, SpecializationCode.Create("code")));
        
            // Act
            var result = _staff.HasSpecializationForOperationType(operationType);
        
            // Assert
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void HasSpecializationForOperationType_DifferentSpecialization_ReturnsFalse()
        {
            // Arrange
            var operationType = CreateTestOperationType(new Specialization("Orthopedicsnew", "", ""));
        
            // Act
            var result = _staff.HasSpecializationForOperationType(operationType);
        
            // Assert
            Assert.That(result, Is.False);
        }

        private Staff CreateTestStaff()
        {
            var firstName = Name.Create("John");
            var lastName = Name.Create("Doe");
            var specialization = new Specialization("Cardiology", "", "");
            var contactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("john@example.com"));
            var licenseNumber = new LicenseNumber("A1234");

            return new Staff(firstName, lastName, specialization, contactInfo, licenseNumber);
        }

        private OperationType CreateTestOperationType(Specialization specializationRequired)
        {
            var operationTypeName = new dddnet8.Domain.OperationTypes.Names.Name("Surgery");
            var status = Status.Active;
            var estimatedDuration = new EstimatedDuration(new System.TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0));

            return new OperationType(Guid.NewGuid(), operationTypeName, status, estimatedDuration, OperationTypeCode.Create("OT0001"))
            {
                SpecializationRequired = specializationRequired
            };
        }*/
    }
}