using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Patients.VO.Name;

namespace YourNamespace.Converters
{
    public class NameConverter : ValueConverter<Name, string>
    {
        public NameConverter()
            : base(
                name => name.ToString(), // Converte o objeto Name para string
                str =>  Name.Create(str) // Converte a string de volta para um objeto Name
            )
        {
        }
    }
}