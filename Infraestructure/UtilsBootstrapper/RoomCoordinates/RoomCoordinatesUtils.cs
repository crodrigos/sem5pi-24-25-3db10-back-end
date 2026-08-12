using dddnet8.Domain.OperationTypes.Names;
using dddnet8.Domain.RoomCoordinates.Domain;
using dddnet8.Domain.RoomCoordinates.Interfaces;
using dddnet8.Domain.Shared;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.UtilsBootstrapper.SurgeryRooms;
using Name = dddnet8.Domain.Patients.V.O.Name;

namespace dddnet8.Infraestructure.UtilsBootstrapper.RoomCoordinates;

public class RoomCoordinatesUtils
{
    private readonly IRoomCoordinateRepository _roomCoordinateRepository;
    
    private readonly SurgeryRoomsUtils _surgeryRoomsUtils;

    public RoomCoordinatesUtils(IRoomCoordinateRepository roomCoordinateRepository, SurgeryRoomsUtils surgeryRoomsUtils)
    {
        _roomCoordinateRepository = roomCoordinateRepository;
        
        _surgeryRoomsUtils = surgeryRoomsUtils;
    }
    
    public async Task InitializeRoomCoordinatesAsync(){
        var rooms = await _roomCoordinateRepository.GetAllAsync();

        if (!rooms.Any())
        {
            await SaveSurgeryRoomCoordinates(await CreateSurgeryRoomCoordinates(null,(0,0), (3,4), 1, await _surgeryRoomsUtils.GetSurgeryRoom("R0001")));
            await SaveSurgeryRoomCoordinates(await CreateSurgeryRoomCoordinates(null,(5,0), (5,3), 3, await _surgeryRoomsUtils.GetSurgeryRoom("R0002")));
            await SaveSurgeryRoomCoordinates(await CreateSurgeryRoomCoordinates(null,(0,4), (3,2), 3, await _surgeryRoomsUtils.GetSurgeryRoom("R0003")));
            
            await SaveSurgeryRoomCoordinates(await CreateSurgeryRoomCoordinates("R0004", (5,3), (5,3), 3, null));
            await SaveSurgeryRoomCoordinates(await CreateSurgeryRoomCoordinates("R0005", (0,8), (5,2), 0, null));
            await SaveSurgeryRoomCoordinates(await CreateSurgeryRoomCoordinates("R0006", (5,8), (5,2), 0, null));
        } 
    }

    private async Task SaveSurgeryRoomCoordinates(RoomCoordinate createSurgeryRoomCoordinates)
    {
        await _roomCoordinateRepository.AddRoomCoordinateAsync(createSurgeryRoomCoordinates);
    }

    private async Task<RoomCoordinate> CreateSurgeryRoomCoordinates(string? roomNumber, (int, int) position, (int, int) dimension, int doorDirection, SurgeryRoom? surgeryRoom)
    {
        
        RoomNumber finalRoomNumber = roomNumber == null ? surgeryRoom!.RoomNumber : new RoomNumber(roomNumber);

        return new RoomCoordinate(
            finalRoomNumber,
            Coordinate.Create(position.Item1, position.Item2),
            Dimensions.Create(dimension.Item1, dimension.Item2),
            DoorDirection.Create(doorDirection));
    }
}