using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.SurgeryRooms;

namespace dddnet8.Domain.SurgeryRooms.Services;

public class SurgeryRoomService : ISurgeryRoomService {
    
    private readonly ISurgeryRoomRepository _surgeryRoomRepository;
    private readonly IMaintenanceSlotRepository _maintenanceSlotRepository;
    private readonly ILogger<SurgeryRoomService> _logger; // Injeção do ILogger



    public SurgeryRoomService(ISurgeryRoomRepository surgeryRoomRepository,
        IMaintenanceSlotRepository maintenanceSlotRepository, ILogger<SurgeryRoomService> logger)
    {
        _surgeryRoomRepository = surgeryRoomRepository;
        _maintenanceSlotRepository = maintenanceSlotRepository;
        _logger = logger;
    }
    
   public async Task<bool> CheckIfRoomIsAvailableForDateAndHour(DateTime appointmentDate, string surgeryRoom, string surgeryStartTime,
        OperationType finalTimeForSurgery)
{
    try
    {
        var roomNumber = new RoomNumber(surgeryRoom);

        var surgeryRoomMaintenanceSlots = await _maintenanceSlotRepository.GetOccupiedSlotsByDate(DateOnly.FromDateTime(appointmentDate), roomNumber);
        
        if (surgeryRoomMaintenanceSlots == null)
        {
            _logger.LogWarning($"Nenhum slot de manutenção encontrado para a sala {surgeryRoom} na data {appointmentDate.ToShortDateString()}.");
            return true; 
        }

        double surgeryStartTimeSpan = TimeSpan.Parse(surgeryStartTime).TotalMinutes;

        double surgeryEndTimeSpan = surgeryStartTimeSpan + finalTimeForSurgery.EstimatedDuration.GetTotalMinutesEstimatedDuration();

        foreach (var surgeryRoomMaintenanceSlot in surgeryRoomMaintenanceSlots)
        {
            double maintenanceStartTimeTotalMinutes = surgeryRoomMaintenanceSlot.StartTime.TotalMinutes;
            double maintenanceEndTimeTotalMinutes = surgeryRoomMaintenanceSlot.EndTime.TotalMinutes;

            if ((surgeryStartTimeSpan < maintenanceEndTimeTotalMinutes && surgeryEndTimeSpan > maintenanceStartTimeTotalMinutes) ||
                (surgeryStartTimeSpan >= maintenanceStartTimeTotalMinutes && surgeryStartTimeSpan < maintenanceEndTimeTotalMinutes))
            {
                _logger.LogWarning($"Conflito de horário: A sala {surgeryRoom} já está ocupada para manutenção no período da cirurgia " +
                                    $"({surgeryStartTime} - {finalTimeForSurgery.EstimatedDuration}) na data {appointmentDate.ToShortDateString()}.");
                return false; // A sala não está disponível
            }
        }
        return true;
    }
    catch (FormatException ex)
    {
        _logger.LogError($"Erro ao converter a hora de início da cirurgia: {ex.Message}");
        return false;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Erro ao verificar a disponibilidade da sala {surgeryRoom} para a data {appointmentDate.ToShortDateString()}: {ex.Message}");
        return false;
    }
}

  
}