using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dddnet8.Domain.Patients.VO.Name;
using YourNamespace.Domain;

namespace YourNamespace.Converters
{
    public class MedicalConditionConverter : ValueConverter<MedicalCondition, string>
    {
        public MedicalConditionConverter()
            : base(
                mc => mc.ConditionName, // Converte MedicalCondition para string
                str => new MedicalCondition(str) // Converte string de volta para MedicalCondition
            )
        {
        }
    }
}