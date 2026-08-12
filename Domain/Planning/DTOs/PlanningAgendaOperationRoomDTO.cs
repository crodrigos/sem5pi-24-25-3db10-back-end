namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningAgendaOperationRoomDTO
{
    public string RoomNumber { get; set; }
    
    public string Date { get; set; }

    public List<string> Schedule { get; set; }

    public PlanningAgendaOperationRoomDTO(string roomNumber, DateOnly date, List<string> schedule)
    {
        RoomNumber = roomNumber;
        Date = int.Parse(date.ToString("yyyyMMdd")).ToString();
        Schedule = schedule;
    }

    public PlanningAgendaOperationRoomDTO()
    {
        Schedule = new List<string>();
    }
    
}