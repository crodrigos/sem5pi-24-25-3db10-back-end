using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.PlanningModuleNotifications.DTOs;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.PlanningModuleNotifications;

public class PlanningMapper
{
    public static PlanningSurgeryDTO ToPlanningSurgeryDTO(OperationType operationType)
    {
        var estimatedDuration = operationType.EstimatedDuration.ToString();

        var parts = estimatedDuration.Split(',');

        return new PlanningSurgeryDTO(operationType.OperationTypeCode._OperationTypeCode.ToLower()!,
            parts[0],
            parts[1],
            parts[2]
        );
    }

    public static PlanningStaffDTO ToPlanningStaffDTO(Staffs.Staff staff,
        List<OperationTypeCode> operationTypeCodes)
    {
        // Converte a lista de OperationTypeCode para uma lista de strings (os códigos das operações)
        var operationTypeCodeStrings = operationTypeCodes
            .Select(otc => otc._OperationTypeCode.ToLower())
            .ToList();

        return new PlanningStaffDTO(
            staff.LicenseNumber.Value.ToLower(),
            staff.GetRole(staff.LicenseNumber.Value).ToLower(),
            staff.Specialization.Name.ToString().ToLower(),
            operationTypeCodeStrings
        );
    }
    
    public static PlanningAllDataForSchedulingDTO ToPlanningAllDataForScheduling(IEnumerable<PlanningSurgeryDTO> allSurgeries, IEnumerable<PlanningStaffDTO> allStaff, IEnumerable<PlanningTimetableDTO> allTimetables, IEnumerable<PlanningAssignmentSurgeryDTO> allAssignmentSurgery, IEnumerable<PlanningSurgeryIdDTO> allSurgeriesId, IEnumerable<PlanningAgendaStaffDTO> allAgendaStaff, IEnumerable<PlanningAgendaOperationRoomDTO>? allAgendaOperationRoom)
    {
        return new PlanningAllDataForSchedulingDTO()
        {
            // Mapeando cada coleção para as propriedades correspondentes no DTO
            Cirurgias = allSurgeries.ToList(),
            Staff = allStaff.ToList(),
            Timetable = allTimetables.ToList(),
            Surgery_id = allSurgeriesId.ToList(),
            Agenda_staff = allAgendaStaff.ToList(),
            Assignment_surgery = allAssignmentSurgery.ToList(),
            Agenda_operation_room = allAgendaOperationRoom?.ToList() 
        };
    }




    public static PlanningTimetableDTO ToPlanningTimetableDTO(Timetables.Timetable timetable)
    {
        return new PlanningTimetableDTO(
            timetable.LicenseNumber.Value.ToLower(),
            timetable.DateShift.Date,
            timetable.TimeShift.Entrance.ToString(),
            timetable.TimeShift.Exit.ToString()
        );
    }

    public static PlanningSurgeryIdDTO ToPlanningSurgeryIdDTO(OperationRequest operationRequest)
    {
        return new PlanningSurgeryIdDTO(operationRequest.OperationRequestCode._operationRequestCode.ToLower(),
            operationRequest.OperationTypeId._OperationTypeCode.ToLower());
    }

    public static PlanningAssignmentSurgeryDTO ToAssignmentSurgeryDTO(string operationRequestCode, string doctorIdValue)
    {
        return new PlanningAssignmentSurgeryDTO(operationRequestCode.ToLower(), doctorIdValue.ToLower());
    }

    public static PlanningAgendaRoomsOccupationDTO ToPlanningAgendaRoomsOccupationDTO(int width, int length,
        List<RoomDto> rooms)
    {
        var roomDtos = rooms.Select(room => new RoomDto
        {
            X = room.X,
            Y = room.Y,
            Width = room.Width,
            Length = room.Length,
            DoorDirection = room.DoorDirection,
            IsOccupied = room.IsOccupied,
            RoomName = room.RoomName
        }).ToList();

        return new PlanningAgendaRoomsOccupationDTO
        {
            Width = width,
            Length = length,
            Rooms = roomDtos,

        };
    }

    public static List<RoomDto> FromPlanningAgendaRoomsOccupationDTO(PlanningAgendaRoomsOccupationDTO dto)
    {
        return dto.Rooms.Select(roomDto => new RoomDto()
        {
            X = roomDto.X,
            Y = roomDto.Y,
            Width = roomDto.Width,
            Length = roomDto.Length,
            DoorDirection = roomDto.DoorDirection,
            IsOccupied = roomDto.IsOccupied
        }).ToList();
    }


}  

