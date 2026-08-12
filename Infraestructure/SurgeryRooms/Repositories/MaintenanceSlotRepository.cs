using System.Collections;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.SurgeryRooms;

public class MaintenanceSlotRepository: BaseRepository<MaintenanceSlot, Guid>, IMaintenanceSlotRepository {
    
private readonly ApplicationDbContext _context;



public MaintenanceSlotRepository(ApplicationDbContext dbContext) : base(dbContext.MaintenanceSlot)
{
    _context = dbContext;
}

public async Task Add(MaintenanceSlot maintenanceSlot)
{
    await _context.AddAsync(maintenanceSlot);
    await _context.SaveChangesAsync();
}

public async Task<IEnumerable<MaintenanceSlot>> GetBySurgeryRoomId(RoomNumber surgeryRoomNumber)
{
    return await _context.MaintenanceSlot.Where(ea => ea.SurgeryRoomNumber.Value == surgeryRoomNumber.Value)  // Comparar pelos valores do RoomNumber
        .ToListAsync();
}

public Task<List<MaintenanceSlot>> GetAllAsync()
{
    return _context.MaintenanceSlot.ToListAsync();
}

public async Task<List<MaintenanceSlot>> GetOccupiedSlotsByDate(DateOnly appointmentDate, RoomNumber surgeryRoomRoomNumber) {
    return await _context.MaintenanceSlot.Where(ms =>
            DateOnly.FromDateTime(ms.Date) == appointmentDate && ms.SurgeryRoomNumber == surgeryRoomRoomNumber).OrderBy(ms => ms.Date).ToListAsync();
}

public async Task<MaintenanceSlot?> GetByRoomDateAndTime(DateOnly dateTime, RoomNumber roomNumber, TimeSpan parse, TimeSpan timeSpan)
{
    return await _context.MaintenanceSlot.Where(ms => DateOnly.FromDateTime(ms.Date) == dateTime && ms.SurgeryRoomNumber == roomNumber && ms.StartTime == parse && ms.EndTime == timeSpan).FirstOrDefaultAsync(); 
}
}