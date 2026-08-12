using dddnet8.Domain.Patients.DataModel;
using dddnet8.Domain.Shared;
using dddnet8.Domain.SystemUsers;

namespace App.Onion.Domain.Interfaces.PatientRepository;

public interface IPatientRepository : IRepository<PatientDataModel, Guid>
{
    Task AddPatientAsync(Patient patient);
    
    IEnumerable<Patient> GetAll();
    Task<int> GetPatientSize();
    Task<IEnumerable<Patient>> SearchPatientsByFiltersAsync(PatientCriteria criteria);
    Task<Patient?> GetPatientByMedicalRecordNumber(MedicalRecordNumber create);
    Task UpdatePatientDataAsync(Patient patient);
    
    Task<Patient?> GetPatientByEmailAddress(EmailAddress address);
    Task<List<Patient>> GetPatientsMarkedForDeletionAsync();
    Task RemovePatientAsync(Patient patient);
}