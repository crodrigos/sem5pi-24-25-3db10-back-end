using App.Onion.Application.Dtos;

namespace App.Onion.Application.Interfaces;

public interface IPatientService
{
    Task<PatientDto> CreatePatient(CreatePatientDTO patientDto);
    Task<IEnumerable<PatientDto>?> SearchPatientsByFilters(PatientCriteria criteria);
    Task<PatientDto> UpdatePatientData(PatientCriteria patientDto, string medicalRecordNumber);
    Task<(bool result, string message)> ValidatePatientForDeletion(string medicalRecordNumber);
    Task MarkPatientForDeletion(string medicalRecordNumber);
    Task MarkPatientAssociatedDataForDeletion(string medicalRecordNumber);
    Task<PatientDto> GetPatientByUsername(string username);
    Task<(bool result, string error)> RequestDpoToDeleteMyAccount(string medicalRecordNumber);
}