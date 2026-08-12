using App.Onion.Domain.V.O.Patient;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Specializations.DTO;
using dddnet8.Domain.Specializations.Interfaces;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.Services;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using Moq;
using NUnit.Framework;
using SurgicalManagement.Domain.Domain;


namespace dddnet8.Tests.Domain.Staffs;

[TestFixture]
public class StaffServiceTestsWithIso
{
    [SetUp]
    public void Setup()
    {
        _staffRepositoryMock = new Mock<IStaffRepository>();
        _licenseNumberGeneratorMock = new Mock<ILicenseNumberGenerator>();
        _emailServiceMock = new Mock<IEmailService>();
        _staffLogServiceMock = new Mock<ILogService<Staff>>();
        _specializationMock = new Mock<ISpecializationRepository>();

        _staffService = new StaffService(_staffRepositoryMock.Object, _licenseNumberGeneratorMock.Object,
            _emailServiceMock.Object, _staffLogServiceMock.Object, _specializationMock.Object);
    }

    private Mock<IStaffRepository> _staffRepositoryMock;
    private Mock<ILicenseNumberGenerator> _licenseNumberGeneratorMock;
    private Mock<IEmailService> _emailServiceMock;
    private StaffService _staffService;
    private Mock<ILogService<Staff>> _staffLogServiceMock;
    private Mock<ISpecializationRepository> _specializationMock;

    /*
    [Test]
    public async Task CreateStaffAsync_ValidDto_CreatesAndReturnsStaffDto()  
    {
        // Arrange
        var createStaffDto = new CreateStaffDto("John", "Doe", new SpecializationDto("Cardiology", ""),
            new ContactInfoDto("932932932", "john@example.com"), "Doctor");

        var expectedLicenseNumber = new LicenseNumber("L1234");
        _licenseNumberGeneratorMock.Setup(g => g.GenerateLicenseNumber(It.IsAny<UserRole>()))
            .Returns(expectedLicenseNumber);

        _staffRepositoryMock.Setup(r => r.AddStaffAsync(It.IsAny<Staff>())).Returns(Task.CompletedTask);

        // Act
        var result = await _staffService.CreateStaffAsync(createStaffDto);

        // Assert
        _staffRepositoryMock.Verify(r => r.AddStaffAsync(It.IsAny<Staff>()), Times.Once);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.FullName, Is.EqualTo("John Doe"));
        Assert.That(result.Specialization, Is.EqualTo("Cardiology"));
    }

    [Test]
    public void CreateStaffAsync_NullDto_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(async () => await _staffService.CreateStaffAsync(null), Throws.ArgumentNullException);
    }

    [Test]
    public async Task UpdateStaffData_ValidData_UpdatesStaff()
    {
        // Arrange
        var staffCriteria = new StaffCriteria
        {
            FirstName = "Jane",
            LastName = "Smith",
            Specialization = new SpecializationDto("Pediatrics", ""),
            ContactInformation = new ContactInfoDto("987654321", "jane@example.com")
        };

        var licenseNumber = "L1234";
        var staff = CreateTestStaff(new Specialization("Pediatrics", ""), licenseNumber);
        _staffRepositoryMock.Setup(r => r.GetByLicenseNumberAsync(It.IsAny<LicenseNumber>())).ReturnsAsync(staff);

        // Act
        var result = await _staffService.UpdateStaffData(staffCriteria, licenseNumber);

        // Assert
        _staffRepositoryMock.Verify(r => r.UpdateStaffDataAsync(It.IsAny<Staff>()), Times.Once);
        Assert.That(result.FullName, Is.EqualTo("Jane Smith"));
        Assert.That(result.Specialization, Is.EqualTo("Pediatrics"));
    }

    [Test]
    public async Task ValidateStaffForDeletion_StaffFound_ReturnsEligibilityForDeletion()
    {
        // Arrange
        var staff = CreateTestStaff(new Specialization("Cardiology", ""), "L1234");
        staff.MarkForDeletion();
        _staffRepositoryMock.Setup(r => r.GetByLicenseNumberAsync(It.IsAny<LicenseNumber>())).ReturnsAsync(staff);

        // Act
        var (result, message) = await _staffService.ValidateStaffForDeletion("L1234");

        // Assert
        Assert.That(result, Is.False);
        Assert.That(message, Is.EqualTo("Staff is already marked for deletion."));
    }

    [Test]
    public async Task ValidateStaffForDeletion_StaffNotFound_ReturnsStaffNotFound()
    {
        // Arrange
        _staffRepositoryMock.Setup(r => r.GetByLicenseNumberAsync(It.IsAny<LicenseNumber>())).ReturnsAsync((Staff)null);

        // Act
        var (result, message) = await _staffService.ValidateStaffForDeletion("L9999");

        // Assert
        Assert.That(result, Is.False);
        Assert.That(message, Is.EqualTo("Staff not found."));
    }

    [Test]
    public async Task MarkStaffForDeletion_ValidLicenseNumber_MarksStaffAsDeleted()
    {
        // Arrange
        var staff = CreateTestStaff(new Specialization("Cardiology", ""), "L1234");
        _staffRepositoryMock.Setup(r => r.GetByLicenseNumberAsync(It.IsAny<LicenseNumber>())).ReturnsAsync(staff);

        // Act
        await _staffService.MarkStaffForDeletion("L1234");

        // Assert
        _staffRepositoryMock.Verify(r => r.UpdateStaffDataAsync(It.IsAny<Staff>()), Times.Once);
        Assert.That(staff.DeletionStatus.IsToDelete, Is.True);
    }

    private Staff CreateTestStaff(Specialization specialization, string licenseNumber)
    {
        return new Staff(
            Name.Create("John"),
            Name.Create("Doe"),
            specialization,
            new ContactInfo(PhoneNumber.Create("351932932932"), EmailAddress.Create("john@example.com")),
            new LicenseNumber(licenseNumber)
        );
    }
*/
}