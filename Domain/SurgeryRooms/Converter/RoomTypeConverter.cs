using dddnet8.Domain.SurgeryRooms.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.SurgeryRooms.Converter;

public class RoomTypeConverter : ValueConverter<RoomType, string>
{
    public RoomTypeConverter() : base(
        status => status.ToString(), 
        value => Enum.Parse<RoomType>(value)) 
    {
    }
}