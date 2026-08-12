using dddnet8.Domain.Shared;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.SurgeryRooms;

public class MaintenanceSlot : Entity<Guid>
{

    // Número da sala de cirurgia
    public RoomNumber SurgeryRoomNumber { get; private set; }

    // Data do slot de manutenção
    public DateTime Date { get; private set; }

    // Horário de início e fim da manutenção
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    // Construtor protegido (para uso pelo ORM)
    protected MaintenanceSlot() : base(Guid.NewGuid()) { }

    // Construtor principal
    public MaintenanceSlot(
        RoomNumber surgeryRoomNumber,
        DateTime date,
        TimeSpan startTime,
        TimeSpan endTime
    ) : base(Guid.NewGuid())
    {
        // Validações
        if (startTime >= endTime)
            throw new ArgumentException("StartTime must be before EndTime.");

        Id = Guid.NewGuid();
        SurgeryRoomNumber = surgeryRoomNumber;
        Date = date.Date; // Garantir apenas a parte da data
        StartTime = startTime;
        EndTime = endTime;
    }

    // Propriedade para obter a duração
    public TimeSpan Duration => EndTime - StartTime;

    // Verificar conflitos com outro MaintenanceSlot
    public bool ConflictsWith(MaintenanceSlot other)
    {
        if (other == null || other.Date != this.Date)
            return false; // Sem conflito se as datas forem diferentes

        return StartTime < other.EndTime && EndTime > other.StartTime;
    }

    // ToString para representar o MaintenanceSlot
    public override string ToString()
    {
        return $"Room {SurgeryRoomNumber} on {Date:yyyy-MM-dd} from {StartTime:hh\\:mm\\:ss} to {EndTime:hh\\:mm\\:ss}";
    }
}
