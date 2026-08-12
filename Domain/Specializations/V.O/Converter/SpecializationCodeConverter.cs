using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dddnet8.Domain.Patients.V.O;

namespace YourNamespace.Converters
{
    public class SpecializationCodeConverter : ValueConverter<SpecializationCode, string>
    {
        public SpecializationCodeConverter() 
            : base(
                specializationCode => specializationCode.Code, // Converte de SpecializationCode para string
                code => SpecializationCode.Create(code) // Converte de string para SpecializationCode
            )
        {
        }
    }
}