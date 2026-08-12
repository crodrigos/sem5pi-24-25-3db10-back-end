using dddnet8.Domain.Shared;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.SurgeryRooms;

public class SurgeryRoom : Entity<Guid>, IAggregateRoot
{
    public RoomNumber RoomNumber { get; private set; }
    
    public RoomType RoomType { get; private set; }
    
    public RoomCapacity RoomCapacity { get; private set; }
    
    public RoomStatus RoomStatus { get; private set; }
    
    protected SurgeryRoom(): base(Guid.NewGuid()){}
    
    public SurgeryRoom(RoomNumber roomNumber, RoomType roomType, RoomCapacity roomCapacity, RoomStatus roomStatus) : base(Guid.NewGuid())
    {
        RoomNumber = roomNumber;
        RoomType = roomType;
        RoomCapacity = roomCapacity;
        RoomStatus = roomStatus;
    }
}