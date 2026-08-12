namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningAgendaRoomsOccupationDTO
{
    public int Width { get; set; }
    public int Length { get; set; }
    public List<RoomDto> Rooms { get; set; }
    
    
}

public class RoomDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public int DoorDirection { get; set; }
    public bool? IsOccupied { get; set; } 
    
    public string RoomName { get; set; }
}