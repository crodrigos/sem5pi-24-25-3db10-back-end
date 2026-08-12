using dddnet8.Domain.SurgeryRooms.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.SurgeryRooms.Converter;

public class RoomCapacityConverter : ValueConverter<RoomCapacity, int>
{
    public RoomCapacityConverter() : base(
        roomCapacity => roomCapacity.Value, 
        value => new RoomCapacity(value)) 
    {
    }
}