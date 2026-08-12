using dddnet8.Domain.SurgeryRooms.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.SurgeryRooms.Converter;

public class RoomStatusConverter : ValueConverter<RoomStatus, string>
{
    public RoomStatusConverter() : base(
        status => status.ToString(), 
        value => Enum.Parse<RoomStatus>(value)) 
    {
    }
}