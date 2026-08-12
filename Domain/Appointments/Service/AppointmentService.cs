using App.Onion.Domain.Interfaces.PatientRepository;
using dddnet8.Domain.Appointments.V.O;
using dddnet8.Domain.Appointments.DTO;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.AssignedStaff.Interfaces;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.PlanningModuleNotifications;
using dddnet8.Domain.PlanningModuleNotifications.DTOs;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.DTO;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.Mapper;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Domain.Timetable;
using dddnet8.Infraestructure.OperationTypes;

namespace dddnet8.Domain.Appointments.Service;

public class AppointmentService : IAppointmentService
{
    private readonly IOperationRequestRepository _operationRequestRepository;
    private readonly IOperationTypeRepository _operationTypeRepository;
    private readonly IPlanningService _planningService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ISurgeryRoomRepository _surgeryRoomRepository;
    private readonly ISurgeryRoomService _surgeryRoomService;
    private readonly ITimeSlotService _timeSlotService;
    private readonly IMaintenanceSlotRepository _maintenanceSlotRepository;
    private readonly ITimeAssignedStaffRepository _assignedStaffRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly ITimetableRepository _timetableRepository;


    public AppointmentService(
        IOperationRequestRepository operationRequestRepository,
        IOperationTypeRepository operationTypeRepository,
        IPlanningService planningService,
        IAppointmentRepository appointmentRepository,
        ISurgeryRoomRepository surgeryRoomRepository,
        ISurgeryRoomService surgeryRoomService,
        ITimeSlotService timeSlotService,
        IMaintenanceSlotRepository maintenanceSlotRepository,
        ITimeAssignedStaffRepository assignedStaffRepository,
        ITimeSlotRepository timeSlotRepository,
        ITimetableRepository timetableRepository)
    {

        _operationRequestRepository = operationRequestRepository ??
                                      throw new ArgumentNullException(nameof(operationRequestRepository));
        _operationTypeRepository =
            operationTypeRepository ?? throw new ArgumentNullException(nameof(operationTypeRepository));
        _planningService = planningService ?? throw new ArgumentNullException(nameof(planningService));
        _appointmentRepository =
            appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _surgeryRoomRepository =
            surgeryRoomRepository ?? throw new ArgumentNullException(nameof(surgeryRoomRepository));
        _surgeryRoomService = surgeryRoomService ?? throw new ArgumentNullException(nameof(surgeryRoomService));
        _timeSlotService = timeSlotService ?? throw new ArgumentNullException(nameof(timeSlotService));
        _maintenanceSlotRepository = maintenanceSlotRepository ??
                                     throw new ArgumentNullException(nameof(maintenanceSlotRepository));
        _assignedStaffRepository =
            assignedStaffRepository ?? throw new ArgumentNullException(nameof(assignedStaffRepository));
        _timeSlotRepository = timeSlotRepository ?? throw new ArgumentNullException(nameof(timeSlotRepository));
        _timetableRepository = timetableRepository ?? throw new ArgumentNullException(nameof(timetableRepository));
    }

    public Task CreateAppointment(Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PlanningStaffDTO?>> GetStaffForAppointmenGetDetailsByCode(string operationRequestCode)
    {
        var operationRequest = await _operationRequestRepository.GetByOperationRequestCode(operationRequestCode);

        if (operationRequest == null)
        {
            throw new KeyNotFoundException("Operation request not found");
        }

        var operationTypeCode = operationRequest.OperationTypeId._OperationTypeCode;

        var allStaffs = await _planningService.GetAllStaffAsync();

        Console.WriteLine(allStaffs.ToString());

        var filteredStaffs = allStaffs.Where(staff => staff.OperationTypeCodes.Contains(operationTypeCode.ToLower()));

        return filteredStaffs;

    }

    public async Task<IEnumerable<GetAllOperationRequestForAppointmentDTO>>
        GetOperationRequestsWithoutAppointmentsAsync()
    {
        // Obtenha todas as solicitações de operação
        var allOperationRequests = await _operationRequestRepository.GetAllAsync();

        // Obtenha todos os agendamentos
        var allAppointments = await _appointmentRepository.GetAllAsync();

        var operationRequestCodesWithAppointments = allAppointments
            .Select(appointment => appointment.OperationRequest._operationRequestCode)
            .ToList();



        var operationRequestsWithoutAppointments = allOperationRequests
            .Where(operationRequest =>
                !operationRequestCodesWithAppointments.Contains(operationRequest.OperationRequestCode
                    ._operationRequestCode))
            .Select(operationRequest => new GetAllOperationRequestForAppointmentDTO()


            {
                PatientId = operationRequest.PatientId.Value,
                DoctorId = operationRequest.DoctorId.Value,
                OperationTypeId = operationRequest.OperationTypeId._OperationTypeCode,
                OperationRequestCode = operationRequest.OperationRequestCode._operationRequestCode,
                OperationRequestDescription = operationRequest.OperationDescription.Value
            })
            .ToList();

        return operationRequestsWithoutAppointments;
    }

    public async Task<IEnumerable<SurgeryRoomDTO>> GetAllSurgeryRoomsForAppointment()
    {
        var allSurgeryRooms = await _surgeryRoomRepository.GetAllSurgeryRooms();

        if (allSurgeryRooms == null)
        {
            throw new KeyNotFoundException("No Surgery Rooms in database");
        }

        var surgeryRoomDTOs = allSurgeryRooms.Select(SurgeryRoomMapper.MapToDTO);

        return surgeryRoomDTOs;

    }

    public async Task<bool> CreateAppointmentByDoctorAsync(CreateAppointmentDTO createAppointmentDto)
    {
        try
        {
            Console.WriteLine("----->" + createAppointmentDto.OperationTypeCode);
            // Verifica se o tipo de operação existe
            var finalTimeForSurgery =
                await _operationTypeRepository.GetByOperationTypeCode(
                    OperationTypeCode.Create(createAppointmentDto.OperationTypeCode.Trim().ToUpper()));
            if (finalTimeForSurgery == null)
            {
                throw new KeyNotFoundException("Operation type not found in Appointment Service.");
            }

            // Valida se o horário e a sala estão disponíveis
            var isRoomAvailable = await _surgeryRoomService.CheckIfRoomIsAvailableForDateAndHour(
                createAppointmentDto.AppointmentDate,
                createAppointmentDto.SurgeryRoom,
                createAppointmentDto.SurgeryStartTime,
                finalTimeForSurgery);

            if (!isRoomAvailable)
            {
                throw new InvalidOperationException("The surgery room is not available for the selected time slot.");
            }

            // Verifica a disponibilidade de cada membro da equipe
            foreach (var teamLicenseNumber in createAppointmentDto.TeamLicenseNumbers)
            {
                var isStaffTimetableAvailable = await _timeSlotService.CheckIfStaffTimetableIsAvailable(
                    createAppointmentDto.AppointmentDate,
                    createAppointmentDto.SurgeryStartTime,
                    finalTimeForSurgery.EstimatedDuration,
                    teamLicenseNumber.ToUpper());

                if (!isStaffTimetableAvailable)
                {
                    throw new InvalidOperationException(
                        $"Staff with license number {teamLicenseNumber} is not available for the selected time slot, because of timetable");
                }

                var isStaffTimeSlotAvailable = await _timeSlotService.CheckIfStaffTimeSlotIsAvailable(
                    createAppointmentDto.AppointmentDate,
                    createAppointmentDto.SurgeryStartTime,
                    finalTimeForSurgery.EstimatedDuration,
                    teamLicenseNumber.ToUpper());

                if (!isStaffTimeSlotAvailable)
                {
                    throw new InvalidOperationException(
                        $"Staff with license number {teamLicenseNumber} is not available for the selected time slot.");
                }
            }

            await SaveAppointmentByDoctor(createAppointmentDto, finalTimeForSurgery);

            return true;
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"Operation type not found in Appointment Service.1 -----> {ex.Message}",
                ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(
                $"Operation type not found in Appointment Service.2 -----> {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Operation type not found in Appointment Service.3 -----> {ex.Message}", ex);
        }
    }

    private async Task SaveAppointmentByDoctor(CreateAppointmentDTO createAppointmentDto,
        OperationType finalTimeForSurgery)
    {
        var OperationRequestCode =
            OperationRequests.OperationRequestCode.Create(createAppointmentDto.OperationRequestCode);
        var roomNumber = new RoomNumber(createAppointmentDto.SurgeryRoom);
        var surgeryStart = TimeSpan.Parse(createAppointmentDto.SurgeryStartTime);
        var surgeryFinalTime = TimeSpan.FromMinutes(surgeryStart.TotalMinutes +
                                                    finalTimeForSurgery.EstimatedDuration
                                                        .GetTotalMinutesEstimatedDuration());

        var appointment = new Appointment(OperationRequestCode, roomNumber,
            DateOnly.FromDateTime(createAppointmentDto.AppointmentDate));

        await _appointmentRepository.AddAppointment(appointment);

        var maintanceSlotForAppointment = new MaintenanceSlot(roomNumber, createAppointmentDto.AppointmentDate,
            surgeryStart, surgeryFinalTime);

        await _maintenanceSlotRepository.Add(maintanceSlotForAppointment);

        var timeShiftForTimeSlot = new TimeShift(surgeryStart, surgeryFinalTime);

        foreach (var teamLicenseNumber in createAppointmentDto.TeamLicenseNumbers)
        {
            var StaffLicenseNumber = new LicenseNumber(teamLicenseNumber.ToUpper());

            var assignedStaff = new global::AssignedStaff(appointment.Id, StaffLicenseNumber);

            var timeslot = new TimeSlot(DateOnly.FromDateTime(createAppointmentDto.AppointmentDate), StaffLicenseNumber,
                timeShiftForTimeSlot);

            await _assignedStaffRepository.AddAssignedStaff(assignedStaff);

            await _timeSlotRepository.AddTimeSlot(timeslot);
        }
    }

    public async Task<bool> testTimetable(DateTime dateTime, string LicenseNumber)
    {
        return await _timeSlotService.CheckIfStaffIsAvailable2(dateTime, LicenseNumber);
    }

    public async Task<List<AppointmentDTOList>> GetAllAppointments()
    {
        var allAppointments = await _appointmentRepository.GetAllAsync();

        if (allAppointments == null || !allAppointments.Any())
            return new List<AppointmentDTOList>();

        return allAppointments
            .Select(appointment => new AppointmentDTOList(

                appointment.SurgeryRoom.Value,
                appointment.AppointmentDate.ToDateTime(TimeOnly.MinValue),
                appointment.OperationRequest._operationRequestCode)).ToList();
    }

    public async Task<AppointmentDataDTO?> GetDataForAppointment(string operationRequestCode)
    {

        var appointment =
            await _appointmentRepository.GetAppointmentByOperationRequest(
                OperationRequestCode.Create(operationRequestCode.ToUpper()));

        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found");
        }

        var allStaffAssigned = await _assignedStaffRepository.GetStaffByAppointmentId(appointment.Id);

        return new AppointmentDataDTO(
            surgeryRoom: appointment.SurgeryRoom?.Value ?? "Unknown Room",
            teamLicenseNumbers: allStaffAssigned.Select(staff => staff.AssignedLicenseNumber.Value).ToList(),
            operationRequestCode: appointment.OperationRequest._operationRequestCode
        );
    }

    public async Task<AppointmentDataDTO> UpdateAppointment(UpdateAppointmentDTO updateAppointmentDto)
    {
        var appointment = await _appointmentRepository.GetAppointmentByOperationRequest(
            OperationRequestCode.Create(updateAppointmentDto.OperationRequestCode.ToUpper()));
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found");
        }
        
        var dateTime = updateAppointmentDto.Date.ToDateTime(TimeOnly.MinValue);

        foreach (var l in updateAppointmentDto.LicenseNumbers)
        {
            // Verifica se existe um timetable para cada número de licença na data especificada.
            var timetable =
                await _timetableRepository.GetTimetableByDateAndLicenseNumber(new LicenseNumber(l),
                    updateAppointmentDto.Date);

            if (timetable == null)
            {
                throw new KeyNotFoundException(
                    $"Timetable does not exist for licenseNumber-{l} for date {updateAppointmentDto.Date}");
            }
        }
        
        // Verifica se o slot de manutenção já está em uso para o quarto, data, e horário fornecidos.
        var maintenanceSlot = await _maintenanceSlotRepository.GetByRoomDateAndTime(
            updateAppointmentDto.Date,
            new RoomNumber(updateAppointmentDto.SurgeryRoom),
            TimeSpan.Parse(updateAppointmentDto.StartTime),
            TimeSpan.Parse(updateAppointmentDto.EndTime));

        if (maintenanceSlot != null)
        {
            throw new ArgumentException("The maintenance slot is already in use");
        }

        // Verifica se o slot de tempo está disponível para cada doctor na data e horário fornecidos.
        foreach (var l in updateAppointmentDto.LicenseNumbers)
        {
            var timeslot = await _timeSlotRepository.GetTimeSlotByLicenseNumberAndDateAndTime(updateAppointmentDto.Date,
                new LicenseNumber(l),
                new TimeShift(TimeSpan.Parse(updateAppointmentDto.StartTime),
                    TimeSpan.Parse(updateAppointmentDto.EndTime)));

            if (timeslot == null)
            {
                throw new ArgumentException(
                    $"The timeslot is already in use for licenseNumber - {l} for date {updateAppointmentDto.Date} ");
            }
        }
        
        // DDD Update the appointment using domain logic
        appointment.UpdateStatus(AppointmentStatus.Scheduled);
        appointment.UpdateSurgeryRoom(new RoomNumber(updateAppointmentDto.SurgeryRoom));
        appointment.UpdateDate(updateAppointmentDto.Date);
        
        await _appointmentRepository.UpdateAppointment(appointment);
        
        var allStaffAssigned = await _assignedStaffRepository.GetStaffByAppointmentId(appointment.Id);
        
        return new AppointmentDataDTO(
            surgeryRoom: appointment.SurgeryRoom?.Value ?? "Unknown Room",
            teamLicenseNumbers: allStaffAssigned.Select(staff => staff.AssignedLicenseNumber.Value).ToList(),
            operationRequestCode: appointment.OperationRequest._operationRequestCode
        );
    }
}

