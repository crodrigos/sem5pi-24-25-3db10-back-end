using dddnet8.Domain.Appointments.DTO;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.AssignedStaff.Interfaces;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.PlanningModuleNotifications.DTOs;
using dddnet8.Domain.RoomCoordinates.Interfaces;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.DTO;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Domain.Timetable;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.RequiredStaffs;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.PlanningModuleNotifications;

public class PlanningService : IPlanningService
{
    
    private readonly IOperationTypeRepository _operationTypeRepository;
    private readonly IOperationRequestRepository _operationRequestRepository;
    private readonly IMaintenanceSlotRepository _maintenanceSlotRepository;
    private readonly ISurgeryRoomRepository _surgeryRoomRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IRequiredStaffRepository _requiredStaffRepository;
    private readonly ITimetableRepository _timetableRepository;
    private readonly ITimeSlotService _timeSlotService;
    private readonly ITimeAssignedStaffRepository _assignedStaffRepository;
    private readonly IRoomCoordinateRepository _roomCoordinateRepository;
    
    private readonly IConfiguration _configuration;

    public PlanningService(IOperationTypeRepository operationTypeRepository,
        IOperationRequestRepository operationRequestRepository, 
        IMaintenanceSlotRepository maintenanceSlotRepository,
        ISurgeryRoomRepository surgeryRoomRepository,
        IStaffRepository staffRepository,
        IRequiredStaffRepository requiredStaffRepository,
        ITimetableRepository timetableRepository,
        ITimeSlotService timeSlotService,
        IAppointmentRepository appointmentRepository,
        ITimeAssignedStaffRepository assignedStaffRepository,
        IRoomCoordinateRepository roomCoordinateRepository,
        IConfiguration configuration)
    {
        _operationTypeRepository = operationTypeRepository;
        _operationRequestRepository = operationRequestRepository;
        _maintenanceSlotRepository = maintenanceSlotRepository;
        _requiredStaffRepository = requiredStaffRepository;
        _surgeryRoomRepository = surgeryRoomRepository;
        _staffRepository = staffRepository;
        _timetableRepository = timetableRepository;
        _timeSlotService = timeSlotService;
        _appointmentRepository = appointmentRepository;
        _assignedStaffRepository = assignedStaffRepository;
        _roomCoordinateRepository = roomCoordinateRepository;
        _configuration = configuration;

    }
    
    
    public async Task<PlanningAllDataForSchedulingDTO> GetAllDataForScheduling(DateOnly date, string room){
        var allSurgeries = await GetAllSurgeriesAsync();
        var allStaff = await GetAllStaffAsync();
        var allTimetables = await GetTimetableForStaffAsync(date);
        var allAssignmentSurgery = await GetAllAssignmentsAsync();
        var allSurgeriesId = await GetAllSurgeriesId();
        var allAgendaStaff = await GetAllAgendaStaff(date);
        var allAgendaOperationRoom = await GetAllAgendaOperationRoom(date,room);

        return PlanningMapper.ToPlanningAllDataForScheduling(allSurgeries, allStaff, allTimetables, allAssignmentSurgery, allSurgeriesId, allAgendaStaff, allAgendaOperationRoom); 
    }

    public async Task<IEnumerable<PlanningSurgeryDTO>> GetAllSurgeriesAsync()
    {
        var allOperationTypes = await _operationTypeRepository.GetAllAsync();
        
        var surgeriesDto = allOperationTypes.Select(operationType => PlanningMapper.ToPlanningSurgeryDTO(operationType));

        return surgeriesDto;
    }


    
    public async Task<IEnumerable<PlanningStaffDTO>> GetAllStaffAsync()
    {
        var allStaff = await GetAllStaffInRepository();
    
        var staffDtos = new List<PlanningStaffDTO>();

        foreach (var staff in allStaff)
        {
            var specialization = staff.Specialization;

            var operationTypeCodes = await _requiredStaffRepository.GetOperationTypesBySpecialization(specialization);
            
            var staffDto = PlanningMapper.ToPlanningStaffDTO(staff, operationTypeCodes);

            staffDtos.Add(staffDto);
        }

        return staffDtos;
    }




    public async Task<IEnumerable<PlanningTimetableDTO>> GetTimetableForStaffAsync(DateOnly date)
    {
        var allstafsTimetables = await _timetableRepository.GetAllStaffsForDate(DateTime.Parse(date.ToString("yyyy-MM-dd")));

        var allStaffsDto = allstafsTimetables.Select(PlanningMapper.ToPlanningTimetableDTO);
        
        return allStaffsDto;
    }

    private async Task<List<Staffs.Staff>> GetAllStaffInRepository()
    {
        return await _staffRepository.GetAllAsync();
    }

    public async Task<IEnumerable<PlanningAssignmentSurgeryDTO>> GetAllAssignmentsAsync()
    {
        var operationRequestsNotInAppointments = await OperationRequestsNotInAppointments();
            
        var allAssignmentSurgeryDto = operationRequestsNotInAppointments.Select(op => PlanningMapper.ToAssignmentSurgeryDTO(op.OperationRequestCode._operationRequestCode, op.DoctorId.Value));

        return allAssignmentSurgeryDto;
    }

    public async Task<List<OperationRequest>> OperationRequestsNotInAppointments()
    {
        var allOperationRequests = await _operationRequestRepository.GetAllAsync();

        var allAppointments = await _appointmentRepository.GetAllAsync();
        
        var operationRequestsNotInAppointments = allOperationRequests
            .Where(or => allAppointments.All(ap => ap.OperationRequest != or.OperationRequestCode))
            .ToList();
        return operationRequestsNotInAppointments;
    }

    public async Task<IEnumerable<PlanningSurgeryIdDTO>> GetAllSurgeriesId()
    {
        var operationRequestsNotInAppointments = await OperationRequestsNotInAppointments();

        var allPlanningSurgeryIdDto = operationRequestsNotInAppointments.Select(PlanningMapper.ToPlanningSurgeryIdDTO);

        return allPlanningSurgeryIdDto;
    }

    public async Task<IEnumerable<PlanningAgendaStaffDTO>> GetAllAgendaStaff(DateOnly date)
    {
        var allStaffs = await _staffRepository.GetAllAsync();

        var staffAgendaDTO = new List<PlanningAgendaStaffDTO>();
        
        foreach (var s in allStaffs){
            
            var busySchedule = new List<string>();
            
            var staffAllTimeSlots = await _timeSlotService.GetTimeSlotByLicenseNumberAndDate(date, s.LicenseNumber);

            if (staffAllTimeSlots.Count == 0)
            {
                var staffAgenda = new PlanningAgendaStaffDTO(s.LicenseNumber.Value.ToLower(), date, new List<string>());
                staffAgendaDTO.Add(staffAgenda);
                continue;
            }
            
            foreach (var ts in staffAllTimeSlots){
                var scheduledString =  $"{ts.TimeShift.Entrance:hh\\:mm\\:ss}-{ts.TimeShift.Exit:hh\\:mm\\:ss}-busy";
                
                busySchedule.Add(scheduledString);
            }
            var planningAgenda = new PlanningAgendaStaffDTO(
                s.LicenseNumber.Value.ToLower(),
                date, 
                busySchedule
            );
            staffAgendaDTO.Add(planningAgenda);
        }

        return staffAgendaDTO;
    }

    public async Task<IEnumerable<PlanningAgendaOperationRoomDTO>?> GetAllAgendaOperationRoom(DateOnly date,string room)
    {

        var roomNumber = await _surgeryRoomRepository.GetSurgeryRoom(new RoomNumber(room.ToUpper()));

        if (roomNumber == null) {throw new Exception("SurgeryRoom does not exist");}
        
        var appointentForRoomAndDate = await _appointmentRepository.GetAppointmentByRoomIdAndDate(roomNumber.RoomNumber, date);

        var appointmensAgenda = new List<PlanningAgendaOperationRoomDTO>();

        if (appointentForRoomAndDate == null){
            var agendaEmpty = new PlanningAgendaOperationRoomDTO(room,date,new List<string>());
            appointmensAgenda.Add(agendaEmpty);
            return appointmensAgenda;
        }

        var slotsSchedule = new List<String>();
        
        var occupiedSlots = await _maintenanceSlotRepository.GetOccupiedSlotsByDate(date, new RoomNumber(room));

        foreach (var o in occupiedSlots){
            var formattedSchedule = $"{o.StartTime}-{o.EndTime}-{appointentForRoomAndDate.OperationRequest._operationRequestCode.ToLower()}";
            slotsSchedule.Add(formattedSchedule);
        }
        var finalAgenda = new PlanningAgendaOperationRoomDTO(room,date,slotsSchedule);
        appointmensAgenda.Add(finalAgenda);
        return appointmensAgenda;
    }
    
    public async Task<PlanningAgendaRoomsOccupationDTO> GetAllRoomsOccupationByDate(DateTime date){
       try{
        // Recupera os valores do RoomTemplate do appsettings
        var roomTemplateWidth = _configuration.GetValue<int>("RoomTemplate:width");
        var roomTemplateLength = _configuration.GetValue<int>("RoomTemplate:length");
        
        var operationRooms = await _surgeryRoomRepository.GetAllSurgeryRooms();

        var rooms = new List<RoomDto>();

        foreach (var op in operationRooms){
            
            var roomMaintenanceSlot = await _maintenanceSlotRepository.GetOccupiedSlotsByDate(DateOnly.FromDateTime(date), op.RoomNumber);

            var roomCoordinate = await _roomCoordinateRepository.GetRoomCoordinates(op.RoomNumber);
            
            rooms.Add(new RoomDto
            {
                X = roomCoordinate.Position.X,
                Y = roomCoordinate.Position.Y,
                Width = roomCoordinate.Size.Width, 
                Length = roomCoordinate.Size.Length, 
                DoorDirection = roomCoordinate.DoorDirection.Direction, 
                IsOccupied = roomMaintenanceSlot.Any(),
                RoomName = roomCoordinate.RoomNumber.Value
            });
        }
        var dto = PlanningMapper.ToPlanningAgendaRoomsOccupationDTO(roomTemplateWidth, roomTemplateLength, rooms);

        return dto;
    }catch (Exception ex) {throw new ApplicationException($"Erro ao obter a ocupação das salas de operação: {ex.Message}", ex);}
    }


    public async Task SavePlanning(SavePlanningDto savePlanningDto)
    {
        var Date = DateTime.Parse(savePlanningDto.date);
        
        await SavePlanAppointment(savePlanningDto.surgeries, savePlanningDto.room.ToUpper(), Date);

       await SavePlanMaintenanceSlot(savePlanningDto.surgeries, savePlanningDto.room.ToUpper(), Date);

       await SaveAssignStaffPlan(savePlanningDto.doctors, savePlanningDto.room.ToUpper(), Date);
       
       await SaveStaffTimeSlotPlan(savePlanningDto.doctors, Date);
    }

    private async Task SaveStaffTimeSlotPlan(List<DoctorScheduleDto> doctors,DateTime date)
    {
        var timeSlotListDto = new List<TimeSlotDTO>();

        try{
            foreach (var d in doctors){
                if (d.schedule.Count == 0) {continue;}

                foreach (var s in d.schedule){
                    var staffTimeSlotDto = new TimeSlotDTO(DateOnly.FromDateTime(date), d.doctor_id.ToUpper(), s.start, s.end);
                    timeSlotListDto.Add(staffTimeSlotDto);
                }
            }

            foreach (var t in timeSlotListDto){
                var timeSlot = new TimeSlot(t.DateOnly, t.LicenseNumber, t.TimeShift);
                
                await _timeSlotService.SaveTimeSlot(timeSlot);
            }
        }
        catch (Exception ex) {throw new ArgumentException("Error in SaveStaffTimeSlotPlan: " + ex.Message, ex);}
    } 
    private async Task SaveAssignStaffPlan(List<DoctorScheduleDto> doctors, string room, DateTime date) {

        foreach (var d in doctors){

            foreach (var s in d.schedule){
                if (s.operation.StartsWith("or")){
                    var appointment = await _appointmentRepository.GetAppointmentByRoomIdAndDateAndOperationRequest(new RoomNumber(room), DateOnly.FromDateTime(date), OperationRequestCode.Create(s.operation.ToUpper()));
                    
                    var assignedStaff = new global::AssignedStaff(appointment.Id, new LicenseNumber(d.doctor_id.ToUpper()));
            
                    await _assignedStaffRepository.AddAssignedStaff(assignedStaff);
                }
            }
        }
    }

    private async Task SavePlanAppointment(List<SurgeryDto> surgeries, string room, DateTime date)
    {
        var appointmentsListDto = new List<CreateAppointmentPlanningDTO>();

        foreach (var s in surgeries)
        {
            var appointmentDto = new CreateAppointmentPlanningDTO(room, date, s.surgery_id.ToUpper());
            appointmentsListDto.Add(appointmentDto);
        }

        foreach (var ap in appointmentsListDto)
        {
            var appointment = new Appointment(ap.OperationRequest,ap.RoomNumber,ap.DateOnly);
           await _appointmentRepository.AddAppointment(appointment);
        }
        
    }

    private async Task SavePlanMaintenanceSlot(List<SurgeryDto> surgeries, string room, DateTime date)
    {
        var maitenanceSlotDtoList = new List<MaitenanceSlotDTO>();
             
        foreach (var s in surgeries)
        {
            var maitenanceDTO = new MaitenanceSlotDTO(room, date, s.start_time, s.end_time);
            
            maitenanceSlotDtoList.Add(maitenanceDTO);
        }

        foreach (var m in maitenanceSlotDtoList)
        {

            var maitenanceSlot = new MaintenanceSlot(new RoomNumber(m.RoomId), m.Date, m.StartDuration, m.EndDuration);
            
           await _maintenanceSlotRepository.Add(maitenanceSlot);
        }
    }
}