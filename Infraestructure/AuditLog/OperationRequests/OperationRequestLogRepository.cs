using dddnet8.AuditLog.Entities;

namespace dddnet8.Infraestructure.AuditLog.OperationRequests;

public class OperationRequestLogRepository : ILogRepository<OperationRequestLog>
{
    private readonly ApplicationDbContext _context;

    public OperationRequestLogRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task AddLogAsync(OperationRequestLog logEntry)
    {
        await _context.AddAsync(logEntry);

        await _context.SaveChangesAsync();
    }
}