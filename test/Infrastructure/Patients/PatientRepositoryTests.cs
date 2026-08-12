using App.Onion.Domain.V.O.Patient;
using App.Onion.Infrastructure.Persistence.Repositories;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.SystemUsers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using dddnet8.Infraestructure;
using YourNamespace.Domain;

[TestFixture]
public class PatientRepositoryTests
{
    private ApplicationDbContext _context;
    private PatientRepository _repository;

    [SetUp]
    public void Setup()
    {
        // Configura um banco de dados InMemory para o DbContext
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new PatientRepository(_context);

        // Popula o banco de dados em memória com dados de teste
        _context.Patients.AddRange(
            PatientMapper.ToDataModel(CreateTestPatient1()),
            PatientMapper.ToDataModel(CreateTestPatient2())
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
    public async Task AddPatientAsync_AddsPatientSuccessfully()
    {
        // Arrange
        var patient = CreateTestPatient1();
        
        var patientDataModel = PatientMapper.ToDataModel(patient);

        // Act
        await _repository.AddPatientAsync(patient);

        // Assert
        var patientsInDb = await _context.Patients.ToListAsync();
        Assert.That(patientsInDb.Count, Is.EqualTo(3)); // Já existem 2 pacientes, mais o adicionado.
    }

    [Test]
    public void GetAll_ReturnsAllPatients()
    {
        // Act
        var result = _repository.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().FirstName.ToString(), Is.EqualTo("John"));
    }

    [Test]
    public async Task GetPatientSize_ReturnsCorrectSize()
    {
        // Act
        var size = await _repository.GetPatientSize();

        // Assert
        Assert.That(size, Is.EqualTo(2)); // Existem 2 pacientes no banco de dados
    }

    [Test]
    public async Task SearchPatientsByFiltersAsync_FiltersByFullName()
    {
        // Arrange
        var criteria = new PatientCriteria { FullName = "John Doe" };

        // Act
        var result = await _repository.SearchPatientsByFiltersAsync(criteria);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().FirstName.ToString(), Is.EqualTo("John"));
    }

    [Test]
    public async Task SearchPatientsByFiltersAsync_FiltersByEmail()
    {
        // Arrange
        var criteria = new PatientCriteria { Email = "alejandro@gmail.com" };

        // Act
        var result = await _repository.SearchPatientsByFiltersAsync(criteria);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().ContactInformation.EmailAddress.ToString(), Is.EqualTo("alejandro@gmail.com"));
    }

    [Test]
    public async Task UpdatePatientDataAsync_UpdatesPatientSuccessfully()
    {
        // Arrange
        var patientDataModel = _context.Patients.First();
        
        var patient = PatientMapper.ToDomainModel(patientDataModel);
        
        Console.WriteLine("PatientDATA -<" + patient.ContactInformation.EmailAddress);

        var contactInformation = new ContactInfoDto("999999999", "updated@example.com");
        var patientCriteria = new PatientCriteria(contactInformation : contactInformation);
        
        // Act
        patient.UpdatePatient(patientCriteria);
        
        _context.Patients.Update(PatientMapper.ToDataModel(patient));

        // Assert
        Assert.That(patient.ContactInformation.EmailAddress.ToString(), Is.EqualTo("updated@example.com"));
    }

    [Test]
    public async Task RemovePatientAsync_RemovesPatientSuccessfully()
    {
        // Arrange
        var patient = _context.Patients.First();

        // Act
        await _repository.RemovePatientAsync(PatientMapper.ToDomainModel(patient));

        // Assert
        var patientsInDb = await _context.Patients.ToListAsync();
        Assert.That(patientsInDb.Count, Is.EqualTo(1)); // Havia 2 pacientes, agora 1.
    }

    private Patient CreateTestPatient1()
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

    private Patient CreateTestPatient2()
    {
        var firstName = Name.Create("Jane");
        var lastName = Name.Create("Doe");
        var dob = DateOfBirth.Create(new DateTime(1999, 1, 1));
        var gender = Gender.Female;
        var medicalRecordNumber = MedicalRecordNumber.Create("202410789012");
        var contactInfo = new ContactInfo(PhoneNumber.Create("923211161"), EmailAddress.Create("jane@gmail.com"));
        var emergencyContact = EmergencyContact.Create("John Doe", "987654321");

        return new Patient(firstName, lastName, dob, gender, medicalRecordNumber, contactInfo, emergencyContact);
    }
}
