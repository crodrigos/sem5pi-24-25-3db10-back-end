using App.Onion.Domain.Interfaces.PatientRepository;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.DataModel;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;

namespace App.Onion.Infrastructure.Persistence.Repositories;

public class PatientRepository : BaseRepository<PatientDataModel, Guid>, IPatientRepository
{
    private readonly ApplicationDbContext _context;

    public PatientRepository(ApplicationDbContext dbContext) : base(dbContext.Patients)
    {
        _context = dbContext;
    }

    public async Task AddPatientAsync(Patient patient)
    {
        // N DES COMMIT DISSO AP
        var utcDateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth.Value, DateTimeKind.Utc);
        patient.DateOfBirth = DateOfBirth.Create(utcDateOfBirth);
        await AddAsync(PatientMapper.ToDataModel(patient));
        
        await _context.SaveChangesAsync();
    }

    public IEnumerable<Patient> GetAll()
    {
        var patients =  _context.Patients.ToList();
        return patients.Select(PatientMapper.ToDomainModel).ToList() ;
    }

    public async Task<int> GetPatientSize()
    {
        return await _context.Patients.CountAsync();
    }
    
    

    public async Task<IEnumerable<Patient>> SearchPatientsByFiltersAsync(PatientCriteria criteria)
    {
        IQueryable<PatientDataModel> query = _context.Patients;

        query = ApplyFirstNameFilter(query, criteria.FirstName);
        query = ApplyLastNameFilter(query, criteria.LastName);
        query = ApplyFullNameFilter(query, criteria.FullName);
        query = ApplyEmailFilter(query, criteria.Email);
        query = ApplyPhoneNumberFilter(query, criteria.PhoneNumber);
        query = ApplyMedicalRecordNumberFilter(query, criteria.MedicalRecordNumber);
        query = ApplyDateOfBirthFilter(query, criteria.DateOfBirth);
        query = ApplyGenderFilter(query, criteria.Gender);

        // Convertendo os resultados de `PatientDataModel` para `Patient`
        var patientsDataModels = await query.ToListAsync();
        return patientsDataModels.Select(PatientMapper.ToDomainModel).ToList();
    }
    public async Task<Patient?> GetPatientByMedicalRecordNumber(MedicalRecordNumber medicalRecordNumber)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.MedicalRecordNumber == medicalRecordNumber);
        return PatientMapper.ToDomainModel(patient);
    }
    
    public async Task<Patient?> GetPatientByEmailAddress(EmailAddress emailAddress) 
    {
        var patients = await _context.Patients
            .ToListAsync();  // Carrega todos os pacientes na memória
    
        var patient = patients
            .FirstOrDefault(p => p.ContactInformation.EmailAddress.GetFullEmail() == emailAddress.GetFullEmail());

        return PatientMapper.ToDomainModel(patient);
    }



    

    public async Task UpdatePatientDataAsync(Patient patient)
    {
        var existingPatientDataModel = await GetByMedicalRecordNumber(patient.MedicalRecordNumber);

        if (existingPatientDataModel == null) throw new KeyNotFoundException("Patient not found.");

        await RemovePatientAsync(PatientMapper.ToDomainModel(existingPatientDataModel));

        await AddPatientModelAsync(existingPatientDataModel, patient);

        await _context.SaveChangesAsync();
    }


    private async Task AddPatientModelAsync(PatientDataModel patientDataModel, Patient patient){
        await _context.AddAsync(PatientMapper.ToDataModel(patient, patientDataModel.Id));
        await _context.SaveChangesAsync();
    }


    public async Task<List<Patient>> GetPatientsMarkedForDeletionAsync()
    {
        var patientsList = _context.Patients.ToList();
        
        var patientsMarkedForDeletion = patientsList.Select(PatientMapper.ToDomainModel).ToList();
        
        return patientsMarkedForDeletion.Where(p => p.DeletionStatus.IsToDelete).ToList();
    }

    public async Task RemovePatientAsync(Patient patient)
    {
        try
        {
            var patientdatamodel = await GetByMedicalRecordNumber(patient.MedicalRecordNumber);
            
            _context.Patients.Remove(patientdatamodel);
            
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<PatientDataModel> GetByMedicalRecordNumber(MedicalRecordNumber medicalRecordNumber)
    {
        return await _context.Patients.FirstOrDefaultAsync(p => p.MedicalRecordNumber == medicalRecordNumber);
    }


// Método para filtrar pelo nome
    private IQueryable<PatientDataModel> ApplyFullNameFilter(IQueryable<PatientDataModel> query, string name)
    {
        
        if (!string.IsNullOrEmpty(name))
        {
            return query.Where(p =>EF.Functions.Like((string)p.FullName, $"%{name}%")); 
        }

        return query;
    }
    
    
    private IQueryable<PatientDataModel> ApplyFirstNameFilter(IQueryable<PatientDataModel> query, string? firstName)
    {
        if (!string.IsNullOrEmpty(firstName))
        {
            query = query.Where(p => EF.Functions.Like((string)p.FirstName, $"%{firstName}%"));
        }
        return query; 
    }
    
    private IQueryable<PatientDataModel> ApplyLastNameFilter(IQueryable<PatientDataModel> query, string? lastName)
    {
        if (!string.IsNullOrEmpty(lastName))
        {
            query = query.Where(p => EF.Functions.Like((string)p.LastName, $"%{lastName}%"));
        }
        return query;
    }



    // Método para filtrar pelo email
    private static IQueryable<PatientDataModel> ApplyEmailFilter(IQueryable<PatientDataModel> query, string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            return query.Where(p => p.ContactInformation.EmailAddress.Equals(EmailAddress.Create(email)));
        }

        return query;
    }

    // Método para filtrar pelo número de telefone
    private static IQueryable<PatientDataModel> ApplyPhoneNumberFilter(IQueryable<PatientDataModel> query, string phoneNumber)
    {
        if (!string.IsNullOrEmpty(phoneNumber))
        {
            return query.Where(p => EF.Functions.Like(p.ContactInformation.PhoneNumber.Number, $"%{phoneNumber}%") || 
                                    EF.Functions.Like(p.EmergencyContact.EmergencyContactPhoneNumber.Number, $"%{phoneNumber}%"));
        }

        return query;
    }


    // Método para filtrar pelo número do registro médico
    private static IQueryable<PatientDataModel> ApplyMedicalRecordNumberFilter(IQueryable<PatientDataModel> query,
        string medicalRecordNumber)
    {
        if (!string.IsNullOrEmpty(medicalRecordNumber))
        {
            return query.Where(p => p.MedicalRecordNumber.Equals(MedicalRecordNumber.Create(medicalRecordNumber)));
        }

        return query;
    }

    // Método para filtrar pela data de nascimento
    private static IQueryable<PatientDataModel> ApplyDateOfBirthFilter(IQueryable<PatientDataModel> query, DateTime? dateOfBirth)
    {
        if (dateOfBirth.HasValue)
        {
            var dob = DateOfBirth.Create(dateOfBirth.Value);
            return query.Where(p => p.DateOfBirth.Equals(dob));
        }

        return query;
    }

    // Método para filtrar pelo gênero
    private static IQueryable<PatientDataModel> ApplyGenderFilter(IQueryable<PatientDataModel> query, string gender)
    {
        if (Enum.TryParse<Gender>(gender, true, out var genderValue))
        {
            return query.Where(p => p.Gender.Equals(genderValue));
        }

        return query;
    }
}
