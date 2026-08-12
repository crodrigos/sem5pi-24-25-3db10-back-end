using dddnet8.AuditLog.Entities;
using YourNamespace.GDPR.Entities;

namespace dddnet8.Infraestructure.AuditLog.Staffs;

public class StaffLogRepository : ILogRepository<StaffLog>
{
    private readonly ApplicationDbContext _context;

    public StaffLogRepository(ApplicationDbContext dbContext) 
    {
        _context = dbContext;
    } 
    public async Task AddLogAsync(StaffLog logEntry)
    {
        await _context.AddAsync(logEntry);
        
        await _context.SaveChangesAsync();
    }
}