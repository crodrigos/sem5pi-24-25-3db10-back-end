namespace dddnet8.Domain.Appointments.DTO;

public class AppointmentDTOList
{
    public string OperationRequest { get; set; }

    public string RoomNumber  { get; set; }
    
    public DateOnly DateOnly { get; set; }

    public AppointmentDTOList(string room, DateTime date, string operationRequest)
    {
        OperationRequest = operationRequest;
        RoomNumber = room;
        DateOnly = DateOnly.FromDateTime(date);
    }
}