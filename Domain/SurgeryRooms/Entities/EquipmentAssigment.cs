using dddnet8.Domain.Shared;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.SurgeryRooms;

public class EquipmentAssignment : Entity<Guid>
{
    
    public Guid SurgeryRoomId { get; private set; }  // A chave estrangeira de SurgeryRoom (Guid)
    public RoomNumber SurgeryRoomRumber { get; private set; } // FK para SurgeryRoom
    public string EquipmentName { get; private set; } // Nome do equipamento

    protected EquipmentAssignment():base(Guid.NewGuid()){}
    public EquipmentAssignment(RoomNumber surgeryRoomNumber, string equipmentName) : base(Guid.NewGuid())
    {
        SurgeryRoomRumber = surgeryRoomNumber;
        EquipmentName = equipmentName;
    }
}