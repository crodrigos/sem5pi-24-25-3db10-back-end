using dddnet8.Domain.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.RoomCoordinates.Converter;

public class DoorDirectionConverter: ValueConverter<DoorDirection, string>
{
    public DoorDirectionConverter()
        : base(
            v => v.ToString(),
            v => DoorDirection.FromString(v)) {
    }
}