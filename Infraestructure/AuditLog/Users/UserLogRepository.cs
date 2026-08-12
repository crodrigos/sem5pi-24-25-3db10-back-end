using dddnet8.AuditLog.Entities;
using YourNamespace.GDPR.Entities;

namespace dddnet8.Infraestructure.AuditLog.Users;

public class UserLogRepository : ILogRepository<UserLog>
{
    private readonly ApplicationDbContext _context;

    public UserLogRepository(ApplicationDbContext dbContext) 
    {
        _context = dbContext;
    } 

    public async Task AddLogAsync(UserLog logEntry)
    {
        await _context.AddAsync(logEntry);
        
        await _context.SaveChangesAsync();
    }
}