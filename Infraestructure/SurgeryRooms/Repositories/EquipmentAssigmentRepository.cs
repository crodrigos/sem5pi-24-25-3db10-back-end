using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Interfaces;
using dddnet8.Domain.SurgeryRooms.V.O;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.SurgeryRooms;

public class EquipmentAssigmentRepository : BaseRepository<EquipmentAssignment, Guid>, IEquipmentAssigmentRepository {
    
private readonly ApplicationDbContext _context;


public EquipmentAssigmentRepository(ApplicationDbContext dbContext) : base(dbContext.EquipmentAssignment)
{
    _context = dbContext;
}


public async Task Add(EquipmentAssignment equipmentAssignment)
{
   await _context.AddAsync(equipmentAssignment);
   await _context.SaveChangesAsync();
}

public async Task<IEnumerable<EquipmentAssignment>> GetBySurgeryRoomId(RoomNumber surgeryRoomNumber)
{
    return await _context.EquipmentAssignment.Where(ea => ea.SurgeryRoomRumber.Value == surgeryRoomNumber.Value)  
        .ToListAsync();
}
}