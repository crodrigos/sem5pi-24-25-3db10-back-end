using dddnet8.AuditLog.Entities;
using YourNamespace.GDPR.Entities;

namespace dddnet8.Infraestructure.AuditLog.Patients;

public class PatientLogRepository : ILogRepository<PatientLog>{
    
    private readonly ApplicationDbContext _context;

    public PatientLogRepository(ApplicationDbContext dbContext) 
    {
        _context = dbContext;
    } 
    public async Task AddLogAsync(PatientLog logEntry)
    {
        await _context.AddAsync(logEntry);
        
        await _context.SaveChangesAsync();
    }
}
