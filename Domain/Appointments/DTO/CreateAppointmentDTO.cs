using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.Appointments.DTO;

public class CreateAppointmentPlanningDTO
{
    public OperationRequestCode OperationRequest { get; set; }

    public RoomNumber RoomNumber  { get; set; }
    
    public DateOnly DateOnly { get; set; }

    public CreateAppointmentPlanningDTO(string room, DateTime date, string operationRequest)
    {
        OperationRequest = OperationRequestCode.Create(operationRequest);
        RoomNumber = new RoomNumber(room);
        DateOnly = DateOnly.FromDateTime(date);
    }
    
}

