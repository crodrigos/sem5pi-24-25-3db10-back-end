using dddnet8.Domain.Appointments.DTO;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.PlanningModuleNotifications.DTOs;
using dddnet8.Domain.SurgeryRooms.DTO;

namespace dddnet8.Domain.Appointments.Interfaces;

public interface IAppointmentService
{
    Task CreateAppointment(Appointment appointment);
    Task<IEnumerable<PlanningStaffDTO?>> GetStaffForAppointmenGetDetailsByCode(string operationRequestCode);
    Task<IEnumerable<GetAllOperationRequestForAppointmentDTO>> GetOperationRequestsWithoutAppointmentsAsync();
    Task<IEnumerable<SurgeryRoomDTO>> GetAllSurgeryRoomsForAppointment();
    Task<bool> CreateAppointmentByDoctorAsync(CreateAppointmentDTO createAppointmentDto);

    Task<bool> testTimetable(DateTime dateTime, string LicenseNumber);
    Task<List<AppointmentDTOList>> GetAllAppointments();
    Task<AppointmentDataDTO?> GetDataForAppointment(string operationRequestCode);
    Task<AppointmentDataDTO> UpdateAppointment(UpdateAppointmentDTO updateAppointmentDto);
}