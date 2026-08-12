using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.SurgeryRooms;

public class SurgeryRoomRepository : BaseRepository<SurgeryRoom, Guid>, ISurgeryRoomRepository {
    
    private readonly ApplicationDbContext _context;
    
    public SurgeryRoomRepository(ApplicationDbContext dbContext) : base(dbContext.SurgeryRoom)
    {
        _context = dbContext;
    }

    public async Task AddSurgeryRoom(SurgeryRoom surgeryRoom){
        
        await _context.AddAsync(surgeryRoom);
        
        await _context.SaveChangesAsync();
    }

    public async Task<List<SurgeryRoom>> GetAllSurgeryRooms()
    {
        return await _context.SurgeryRoom.ToListAsync();
    }

    public async Task<SurgeryRoom?> GetSurgeryRoom(RoomNumber roomNumber)
    {
        return await _context.SurgeryRoom.Where(sr => sr.RoomNumber == roomNumber).FirstOrDefaultAsync();
    }
}