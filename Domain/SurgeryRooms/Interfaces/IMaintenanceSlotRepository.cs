using System.Collections;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.SurgeryRooms.Interfaces;

public interface IMaintenanceSlotRepository
{
    Task Add(MaintenanceSlot maintenanceSlot);
    
    Task<IEnumerable<MaintenanceSlot>> GetBySurgeryRoomId(RoomNumber surgeryRoomNumber);
    Task<List<MaintenanceSlot>> GetAllAsync();
    Task<List<MaintenanceSlot>> GetOccupiedSlotsByDate(DateOnly appointmentDate, RoomNumber surgeryRoomRoomNumber);

    Task<MaintenanceSlot?> GetByRoomDateAndTime(DateOnly dateTime, RoomNumber roomNumber, TimeSpan parse, TimeSpan timeSpan);
}