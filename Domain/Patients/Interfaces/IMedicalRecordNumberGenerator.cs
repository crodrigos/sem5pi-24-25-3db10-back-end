namespace App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;


public interface IMedicalRecordNumberGenerator
{
    Task<MedicalRecordNumber> GenerateMedicalRecordNumber();
}