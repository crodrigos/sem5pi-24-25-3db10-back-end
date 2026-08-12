using dddnet8.Domain.OperationTypes;

namespace dddnet8.Domain.SurgeryRooms.Interfaces;

public interface ISurgeryRoomService
{
    Task<bool> CheckIfRoomIsAvailableForDateAndHour(DateTime appointmentDate, string surgeryRoom, string surgeryStartTime, OperationType finalTimeForSurgery);
}