using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.SurgeryRooms.Interfaces;

public interface IEquipmentAssigmentRepository
{
    Task Add(EquipmentAssignment equipmentAssignment);
    
    Task<IEnumerable<EquipmentAssignment>> GetBySurgeryRoomId(RoomNumber surgeryRoomNumber);
}