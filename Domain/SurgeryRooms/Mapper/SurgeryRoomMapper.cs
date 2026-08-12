using dddnet8.Domain.SurgeryRooms.DTO;

namespace dddnet8.Domain.SurgeryRooms.Mapper;

public class SurgeryRoomMapper
{
    public static SurgeryRoomDTO MapToDTO(SurgeryRoom surgeryRoom)
    {
        if (surgeryRoom == null)
        {
            return null;
        }

        return new SurgeryRoomDTO
        {
            RoomNumber = surgeryRoom.RoomNumber.Value, // Supondo que RoomNumber seja uma classe/valor
            RoomType = surgeryRoom.RoomType.ToString(),     // Supondo que RoomType seja uma classe/valor
            RoomCapacity = surgeryRoom.RoomCapacity.Value // Supondo que RoomCapacity seja uma classe/valor
        };
    }
}
