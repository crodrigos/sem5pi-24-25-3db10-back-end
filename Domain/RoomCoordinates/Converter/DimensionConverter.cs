using dddnet8.Domain.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.RoomCoordinates.Converter;

public class DimensionConverter : ValueConverter<Dimensions, string>
{
    public DimensionConverter()
        : base(
            v => v.ToString(),
            v => Dimensions.FromString(v)) {
    }
}
    
