using System.Collections;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Appointments.Interfaces;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.Appointments;

public class AppointmentRepository : BaseRepository<Appointment, Guid>, IAppointmentRepository {
    
    private readonly ApplicationDbContext _context;


    public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext.Appointment)
    {
        _context = dbContext;
    }

    public async Task AddAppointment(Appointment appointment)
    {
       await _context.AddAsync(appointment);
       await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAppointment(Appointment appointment)
    {
        await _context.SaveChangesAsync();
    }

    public Task<List<Appointment>> GetAppointmentsBySurgeryRoomId(RoomNumber surgeryRoomRoomNumber)
    {
        return _context.Appointment.Where(a => a.SurgeryRoom == surgeryRoomRoomNumber).ToListAsync();
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _context.Appointment.ToListAsync();
    }

    public async Task<Appointment> GetAppointmentByRoomIdAndDate(RoomNumber surgeryRoomRoomNumber, DateOnly date)
    {
        return _context.Appointment.FirstOrDefault(a => a.SurgeryRoom == surgeryRoomRoomNumber && a.AppointmentDate == date);
    }

    public async Task<List<Appointment>> GetAppointmentByRoomIdAndDateList(RoomNumber roomNumber, DateOnly date)
    {
        return await _context.Appointment.Where(a => a.AppointmentDate == date && a.SurgeryRoom == roomNumber).ToListAsync();
    }

    public async Task<Appointment> GetAppointmentByRoomIdAndDateAndOperationRequest(RoomNumber surgeryRoomRoomNumber, DateOnly date,OperationRequestCode operationRequestCode){
        return _context.Appointment.FirstOrDefault(a => a.SurgeryRoom == surgeryRoomRoomNumber && a.AppointmentDate == date && a.OperationRequest == operationRequestCode);
    }

    public async Task<Appointment> GetAppointmentByOperationRequest(OperationRequestCode operationRequest){
        return _context.Appointment.FirstOrDefault(a => a.OperationRequest == operationRequest);
    }
}