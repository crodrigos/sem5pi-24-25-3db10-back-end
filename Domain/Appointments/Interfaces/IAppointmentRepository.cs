using System.Collections;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.Appointments.Interfaces;

public interface IAppointmentRepository
{
    Task AddAppointment(Appointment appointment);
    Task UpdateAppointment(Appointment appointment);
    Task<List<Appointment>> GetAppointmentsBySurgeryRoomId(RoomNumber surgeryRoomRoomNumber);
    Task<List<Appointment>> GetAllAsync();
    Task<Appointment> GetAppointmentByRoomIdAndDate(RoomNumber surgeryRoomRoomNumber, DateOnly date);
    Task<List<Appointment>> GetAppointmentByRoomIdAndDateList(RoomNumber roomNumber, DateOnly date);
    Task<Appointment> GetAppointmentByRoomIdAndDateAndOperationRequest(RoomNumber p0, DateOnly fromDateTime, OperationRequestCode create);
    Task<Appointment> GetAppointmentByOperationRequest(OperationRequestCode create);
}