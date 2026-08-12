using dddnet8.AuditLog.Entities;
using dddnet8.AuditLog.Interfaces;
using SurgicalManagement.Domain.Common;
using YourNamespace.GDPR.Entities;

namespace dddnet8.AuditLog.Services;

public class PatientLogService : ILogService<Patient>
{
    private readonly ILogRepository<PatientLog> _patientLogRepository;

    public PatientLogService(ILogRepository<PatientLog> patientLogRepository)
    {
        _patientLogRepository = patientLogRepository;
    }

    public async Task LogActionAsync(string action, Patient patient)
    {
        var patientLog = new PatientLog(action, patient.FirstName, patient.LastName, 
            patient.DateOfBirth, patient.Gender, 
            patient.MedicalRecordNumber, patient.ContactInformation, 
            patient.DeletionStatus, patient.EmergencyContact);
        
        await _patientLogRepository.AddLogAsync(patientLog);
    }
}
