using dddnet8.Infraestructure.UtilsBootstrapper.Appointments;
using dddnet8.Infraestructure.UtilsBootstrapper.AssignedStaffs;
using dddnet8.Infraestructure.UtilsBootstrapper.MaintanceSlots;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationRequests;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationTypes;
using dddnet8.Infraestructure.UtilsBootstrapper.Patients;
using dddnet8.Infraestructure.UtilsBootstrapper.RequiredStaff;
using dddnet8.Infraestructure.UtilsBootstrapper.RoomCoordinates;
using dddnet8.Infraestructure.UtilsBootstrapper.Specializations;
using dddnet8.Infraestructure.UtilsBootstrapper.Staffs;
using dddnet8.Infraestructure.UtilsBootstrapper.SurgeryRooms;
using dddnet8.Infraestructure.UtilsBootstrapper.SystemUsers;

namespace dddnet8.Infraestructure;

public class ApplicationBootstrapper
{
    private readonly SpecializationsUtils _specializationsUtils;
    private readonly OperationTypeUtils _operationTypeUtils;
    private readonly RequiredStaffUtils _requiredStaffUtils;
    private readonly StaffUtils _staffUtils;
    private readonly SystemUserUtils _systemUserUtils;
    private readonly SurgeryRoomsUtils _surgeryRoomsUtils;
    private readonly PatientUtils _patientUtils;
    private readonly MaintenanceSlotsUtils _maintenanceSlots;
    private readonly OperationRequestUtils _operationRequestUtils;
    private readonly AppointmentUtils _appointmentUtils;
    private readonly RoomCoordinatesUtils _roomCoordinatesUtils;

    public ApplicationBootstrapper(
        SpecializationsUtils specializationsUtils, 
        OperationTypeUtils operationTypeUtils,
        RequiredStaffUtils requiredStaffUtils,
        StaffUtils staffUtils, 
        SystemUserUtils systemUserUtils,
        SurgeryRoomsUtils surgeryRoomsUtils,
        PatientUtils patientUtils,
        MaintenanceSlotsUtils maintenanceSlotsUtils,
        OperationRequestUtils operationRequestUtils,
        AppointmentUtils appointmentUtils,
        RoomCoordinatesUtils roomCoordinatesUtils)
    {
        _specializationsUtils = specializationsUtils;
        _operationTypeUtils = operationTypeUtils;
        _requiredStaffUtils = requiredStaffUtils;
        _staffUtils = staffUtils;
        _systemUserUtils = systemUserUtils;
        _surgeryRoomsUtils = surgeryRoomsUtils;
        _patientUtils = patientUtils;
        _maintenanceSlots = maintenanceSlotsUtils;
        _operationRequestUtils = operationRequestUtils;
        _appointmentUtils = appointmentUtils;
        _roomCoordinatesUtils = roomCoordinatesUtils;
    }

    public async Task Initialize()
    {
        await _specializationsUtils.InitializeSpecializationsAsync();
        await _operationTypeUtils.InitializeOperationTypesAsync();
        await _requiredStaffUtils.InitializeRequiredStaffAsync();
        await _systemUserUtils.InitializeSystemUserAsync();
        await _staffUtils.InitializeStaffAsync();
        await _surgeryRoomsUtils.InitializeSurgeryRoomsAsync();
        await _patientUtils.InitializePatientsAsync();
        await _maintenanceSlots.InitializeMaintenanceSlotsAsync();
        await _operationRequestUtils.InitializeOperationRequestAsync();
        await _appointmentUtils.InitializeAppointmentsAsync();
        await _roomCoordinatesUtils.InitializeRoomCoordinatesAsync();
    }
}


