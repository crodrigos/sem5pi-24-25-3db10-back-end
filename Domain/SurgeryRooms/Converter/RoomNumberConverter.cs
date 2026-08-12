using dddnet8.Domain.SurgeryRooms.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.SurgeryRooms.Converter;

public class RoomNumberConverter : ValueConverter<RoomNumber, string>
{
    public RoomNumberConverter() : base(
        roomNumber => roomNumber.Value, 
        value => new RoomNumber(value)) 
    {
    }
}