namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningAllDataForSchedulingDTO
{
    public List<PlanningSurgeryDTO> Cirurgias { get; set; }
    
    public List<PlanningStaffDTO> Staff { get; set; }
    
    public List<PlanningTimetableDTO> Timetable { get; set; }
    
    public List<PlanningSurgeryIdDTO> Surgery_id { get; set; }
    
    public List<PlanningAgendaStaffDTO> Agenda_staff { get; set; }
    
    public List<PlanningAssignmentSurgeryDTO> Assignment_surgery { get; set; }
    
    public List<PlanningAgendaOperationRoomDTO>? Agenda_operation_room { get; set; }
    
}


