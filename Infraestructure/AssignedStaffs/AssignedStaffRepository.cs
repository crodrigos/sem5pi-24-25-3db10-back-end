using dddnet8.Domain.AssignedStaff.Interfaces;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.AssignedStaffs;

public class AssignedStaffRepository :BaseRepository<AssignedStaff, Guid>, ITimeAssignedStaffRepository
{

    private ApplicationDbContext _context;

    public AssignedStaffRepository(ApplicationDbContext context) : base(context.AssignedStaff)
    {
        _context = context;
    }

    public async Task AddAssignedStaff(AssignedStaff assignedStaff)
    {
        await _context.AssignedStaff.AddAsync(assignedStaff);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AssignedStaff>> GetStaffByAppointmentId(Guid appointmentId){
        return await _context.AssignedStaff.Where(s => s.AppointmentId == appointmentId).ToListAsync();
    }
}