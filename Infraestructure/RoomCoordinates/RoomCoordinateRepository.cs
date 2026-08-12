using dddnet8.Domain.RoomCoordinates.Domain;
using dddnet8.Domain.RoomCoordinates.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.RoomCoordinates;

public class RoomCoordinateRepository : BaseRepository<RoomCoordinate, Guid>, IRoomCoordinateRepository
{
    private readonly ApplicationDbContext _context;

    public RoomCoordinateRepository(ApplicationDbContext dbContext) : base(dbContext.RoomCoordinate)
    {
        _context = dbContext;
    }


    public async Task AddRoomCoordinateAsync(RoomCoordinate roomCoordinate)
    {
        await _context.RoomCoordinate.AddAsync(roomCoordinate);
        await _context.SaveChangesAsync();
    }

    public Task<List<RoomCoordinate>> GetAllRoomCoordinates()
    {
        return _context.RoomCoordinate.ToListAsync();
    }

    public async Task<RoomCoordinate> GetRoomCoordinates(RoomNumber opRoomNumber)
    {
        return await _context.RoomCoordinate.Where(r => r.RoomNumber == opRoomNumber).FirstOrDefaultAsync();
    }
}