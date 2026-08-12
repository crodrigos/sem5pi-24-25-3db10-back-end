using App.Onion.Domain.V.O.Patient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dddnet8.Domain.Patients.VO.Name;

namespace YourNamespace.Converters
{
    public class MedicalRecordNumberConverter : ValueConverter<MedicalRecordNumber, string>
    {
        public MedicalRecordNumberConverter()
            : base(
                m => m.Value, // Converte o MedicalRecordNumber para inteiro
                value => new MedicalRecordNumber(value) // Converte o inteiro de volta para um objeto MedicalRecordNumber
            )
        {
        }
    }
}