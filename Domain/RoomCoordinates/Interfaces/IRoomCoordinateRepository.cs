using dddnet8.Domain.RoomCoordinates.Domain;
using dddnet8.Domain.Shared;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.RoomCoordinates.Interfaces;

public interface IRoomCoordinateRepository : IRepository<RoomCoordinate,Guid>
{
    Task AddRoomCoordinateAsync(RoomCoordinate roomCoordinate);
    Task<List<RoomCoordinate>> GetAllRoomCoordinates();
    Task<RoomCoordinate> GetRoomCoordinates(RoomNumber opRoomNumber);
}