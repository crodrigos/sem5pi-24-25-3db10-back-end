using dddnet8.Domain.PlanningModuleNotifications.DTOs;

namespace dddnet8.Domain.PlanningModuleNotifications;

public interface IPlanningService
{
    Task<IEnumerable<PlanningSurgeryDTO>> GetAllSurgeriesAsync();
    Task<IEnumerable<PlanningStaffDTO>> GetAllStaffAsync(); 
    Task<IEnumerable<PlanningTimetableDTO>> GetTimetableForStaffAsync(DateOnly date);
    Task<IEnumerable<PlanningAssignmentSurgeryDTO>> GetAllAssignmentsAsync();
    Task<IEnumerable<PlanningSurgeryIdDTO>> GetAllSurgeriesId();
    Task<IEnumerable<PlanningAgendaStaffDTO>> GetAllAgendaStaff(DateOnly date);
    Task<IEnumerable<PlanningAgendaOperationRoomDTO>?> GetAllAgendaOperationRoom(DateOnly date, string room);
    Task<PlanningAgendaRoomsOccupationDTO> GetAllRoomsOccupationByDate(DateTime date);
    Task<PlanningAllDataForSchedulingDTO> GetAllDataForScheduling(DateOnly date, string room);
    Task SavePlanning(SavePlanningDto savePlanningDto);
}