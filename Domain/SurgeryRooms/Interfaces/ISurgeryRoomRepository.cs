using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.SurgeryRooms.Interfaces;

public interface ISurgeryRoomRepository
{
    Task AddSurgeryRoom(SurgeryRoom surgeryRoom);

    Task<List<SurgeryRoom>> GetAllSurgeryRooms();
    Task<SurgeryRoom?> GetSurgeryRoom(RoomNumber roomNumber);
}