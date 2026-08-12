using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Infraestructure.UtilsBootstrapper.SurgeryRooms;

public class SurgeryRoomsUtils
{
    private readonly ISurgeryRoomRepository _surgeryRoomRepository;

    public SurgeryRoomsUtils(ISurgeryRoomRepository surgeryRoomRepository)
    {
        _surgeryRoomRepository = surgeryRoomRepository;
    }
    
    public async Task InitializeSurgeryRoomsAsync(){
        var rooms = await _surgeryRoomRepository.GetAllSurgeryRooms();

        if (!rooms.Any())
        {
            await SaveSurgeryRoom(await CreateSurgeryRoom("R0001", 10, RoomStatus.Available, RoomType.OperatingRoom));
            await SaveSurgeryRoom(await CreateSurgeryRoom("R0002", 10, RoomStatus.Available, RoomType.OperatingRoom));
            await SaveSurgeryRoom(await CreateSurgeryRoom("R0003", 10, RoomStatus.Available, RoomType.OperatingRoom));
        } 
    }

    private async Task SaveSurgeryRoom(SurgeryRoom surgeryRoom)
    {
        _surgeryRoomRepository.AddSurgeryRoom(surgeryRoom);
    }

    private async Task<SurgeryRoom> CreateSurgeryRoom(string room, int capacity, RoomStatus status, RoomType type)
    {
        RoomNumber roomNumber = new RoomNumber(room);
        
        RoomCapacity roomCapacity = new RoomCapacity(capacity);
        
        return new SurgeryRoom(roomNumber, type, roomCapacity, status);
        
    }

    public async Task<SurgeryRoom?> GetSurgeryRoom(string room)
    {
       return await _surgeryRoomRepository.GetSurgeryRoom(new RoomNumber(room));
    }
    
}