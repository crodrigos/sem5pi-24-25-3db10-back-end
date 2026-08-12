using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Infraestructure.Staffs;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure;
using Name = dddnet8.Domain.Patients.V.O.Name;

[TestFixture]
public class StaffRepositoryTests
{
    private ApplicationDbContext _context;
    private StaffRepository _repository;

    /*[SetUp]
    public void Setup()
    {
        // Configura um banco de dados InMemory para o DbContext
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new StaffRepository(_context);

        // Popula o banco de dados em memória com dados de teste
        _context.Staff.AddRange(
            CreateTestStaff1(),
            CreateTestStaff2()
        );
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();  // Apaga o banco de dados após cada teste
        _context.Dispose();
    }

    [Test]
    public async Task AddStaffAsync_AddsStaffSuccessfully()
    {
        // Arrange
        var staff = CreateTestStaff3();
        
        // Act
        await _repository.AddStaffAsync(staff);

        // Assert
        var staffInDb = await _context.Staff.ToListAsync();
        Assert.That(staffInDb.Count, Is.EqualTo(3)); // Já existem 2 membros da equipe, mais o adicionado.
    }

    [Test]
    public async Task GetByLicenseNumberAsync_ReturnsCorrectStaff()
    {
        // Arrange
        var staff = _context.Staff.First();
        
        // Act
        var result = await _repository.GetByLicenseNumberAsync(staff.LicenseNumber);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.FullName.ToString(), Is.EqualTo(staff.FullName.ToString()));
    }

    [Test]
    public async Task SearchStaffByFiltersAsync_FiltersByFullName()
    {
        // Arrange
        var criteria = new StaffCriteria { FullName = "John Doe" };

        // Act
        var result = await _repository.SearchStaffByFiltersAsync(criteria);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().FullName.ToString(), Is.EqualTo("John Doe"));
    }

    [Test]
    public async Task UpdateStaffDataAsync_UpdatesStaffSuccessfully()
    {
        // Arrange
        var staff = _context.Staff.First();
        var contactInformation = new ContactInfoDto("999999999", "updated@example.com");
        staff.UpdateStaff(new StaffCriteria { ContactInformation = contactInformation });

        // Act
        await _repository.UpdateStaffDataAsync(staff);

        // Assert
        var updatedStaff = await _context.Staff.FindAsync(staff.Id);
        Assert.That(updatedStaff.ContactInfo.EmailAddress.ToString(), Is.EqualTo("updated@example.com"));
    }

    [Test]
    public async Task RemoveStaffAsync_RemovesStaffSuccessfully()
    {
        // Arrange
        var staff = _context.Staff.First();

        // Act
        await _repository.RemoveStaffAsync(staff);

        // Assert
        var staffInDb = await _context.Staff.ToListAsync();
        Assert.That(staffInDb.Count, Is.EqualTo(1)); // Havia 2 membros da equipe, agora 1.
    }

    private Staff CreateTestStaff1()
    {
        var firstName = Name.Create("John");
        var lastName = Name.Create("Doe");
        var specialization = new Specialization("Cardiology", "");
        var contactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("john@example.com"));
        var licenseNumber = new LicenseNumber("D1234");

        return new Staff(firstName, lastName, specialization, contactInfo, licenseNumber);
    }

    private Staff CreateTestStaff2()
    {
        var firstName = Name.Create("Jane");
        var lastName = Name.Create("Smith");
        var specialization = new Specialization("Pediatrics", "");
        var contactInfo = new ContactInfo(PhoneNumber.Create("923211161"), EmailAddress.Create("jane@example.com"));
        var licenseNumber = new LicenseNumber("N1234");

        return new Staff(firstName, lastName, specialization, contactInfo, licenseNumber);
    }

    private Staff CreateTestStaff3()
    {
        var firstName = Name.Create("Bob");
        var lastName = Name.Create("Brown");
        var specialization = new Specialization("Orthopedics", "");
        var contactInfo = new ContactInfo(PhoneNumber.Create("923211162"), EmailAddress.Create("bob@example.com"));
        var licenseNumber = new LicenseNumber("D2468");

        return new Staff(firstName, lastName, specialization, contactInfo, licenseNumber);
    }*/
}
