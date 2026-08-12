using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dddnet8.Domain.Shared;

namespace App.Domain.SystemUser
{
    public class CoordinateConverter : ValueConverter<Coordinate, string>
    {
        public CoordinateConverter()
            : base(
                v => v.ToString(),
                v => Coordinate.FromString(v)) // Converte a tupla de inteiros de volta para Coordinate
            
        {
        }
    }
}