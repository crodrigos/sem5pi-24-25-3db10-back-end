namespace dddnet8.Domain.SurgeryRooms.DTO;

public class MaitenanceSlotDTO
{
    public string RoomId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartDuration { get; set; }
    public TimeSpan EndDuration { get; set; }

    // Construtor personalizado para a classe
    public MaitenanceSlotDTO(string roomId, DateTime date, int startDuration, int endDuration)
    {
        RoomId = roomId;
        Date = date;
        
        // Convertendo as strings para TimeSpan
        StartDuration = TimeSpan.FromMinutes(startDuration);
        
        EndDuration = TimeSpan.FromMinutes(endDuration);
    }
}