using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.Appointments.V.O;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Infraestructure.UtilsBootstrapper.AssignedStaffs;
using dddnet8.Infraestructure.UtilsBootstrapper.MaintanceSlots;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationRequests;
using dddnet8.Infraestructure.UtilsBootstrapper.Staffs;
using dddnet8.Infraestructure.UtilsBootstrapper.SurgeryRooms;

namespace dddnet8.Infraestructure.UtilsBootstrapper.Appointments;

public class AppointmentUtils
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly OperationRequestUtils _operationRequestUtils;
    private readonly SurgeryRoomsUtils _surgeryRoomsUtils;
    private readonly AssignedStaffUtils _assignedStaffsUtils;
    private readonly MaintenanceSlotsUtils _maintenanceSlotsUtils;
    private readonly StaffUtils _staffUtils;

    public AppointmentUtils(IAppointmentRepository appointmentRepository,
        OperationRequestUtils operationRequestUtils, SurgeryRoomsUtils surgeryRoomsUtils,
        AssignedStaffUtils assignedStaffsUtils, MaintenanceSlotsUtils maintenanceSlotsUtils,
        StaffUtils staffUtils)
    {
        _appointmentRepository = appointmentRepository;
        _operationRequestUtils = operationRequestUtils;
        _surgeryRoomsUtils = surgeryRoomsUtils;
        _assignedStaffsUtils = assignedStaffsUtils;
        _maintenanceSlotsUtils = maintenanceSlotsUtils;
        _staffUtils = staffUtils;
    }
    
    public async Task InitializeAppointmentsAsync(){
        var appointments = await _appointmentRepository.GetAllAsync();

        if (!appointments.Any())
        {
            await SaveAppointment(await CreateAppointment(
                await _surgeryRoomsUtils.GetSurgeryRoom("R0002"),
                await _operationRequestUtils.GetOperationRequest("OR0014"),
                new DateOnly(2024, 10, 28)
                ));
        } 
    }

    private async Task SaveAppointment(Appointment appointment)
    {

        appointment.UpdateStatus(AppointmentStatus.Completed);
        
       await _appointmentRepository.AddAppointment(appointment);

       _maintenanceSlotsUtils.CreateMaintenanceSlotForAppointment(appointment.AppointmentDate, appointment.SurgeryRoom, new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0));

    }

    private async Task<Appointment> CreateAppointment(SurgeryRoom? surgeryRoom, OperationRequest? operationRequest, DateOnly date)
    {
        Console.WriteLine("REQUEST--------->" + operationRequest);
        Console.WriteLine("ROOM--------->" + surgeryRoom);

        return new Appointment(operationRequest!.OperationRequestCode, surgeryRoom!.RoomNumber, date);
    }
}

