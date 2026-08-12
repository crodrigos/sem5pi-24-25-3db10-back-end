using App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;
using App.Onion.Domain.Interfaces.PatientRepository;

namespace App.Onion.Infrastructure.Persistence;

public class MedicalRecordNumberGenerator : IMedicalRecordNumberGenerator
{
    private static IPatientRepository _patientRepository;

    public MedicalRecordNumberGenerator(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    
    public async Task<MedicalRecordNumber> GenerateMedicalRecordNumber()
    {
       
        var count = await _patientRepository.GetPatientSize();

        var now = DateTime.Now;
        var year = now.ToString("yyyy");
        var month = now.ToString("MM");

        var sequentialNumber = (count + 1).ToString("D6");

        // Constrói o MRN no formato YYYYMMnnnnnn
        var medicalRecordNumber = $"{year}{month}{sequentialNumber}";

        return new MedicalRecordNumber(medicalRecordNumber);
    }
}