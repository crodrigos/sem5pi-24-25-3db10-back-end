using dddnet8.AuditLog.Entities;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Staffs;

namespace dddnet8.AuditLog.Services;

public class StaffLogService : ILogService<Staff>
{
    
    private readonly ILogRepository<StaffLog> _staffLogRepository;

    public StaffLogService(ILogRepository<StaffLog> staffLogRepository)
    {
        _staffLogRepository = staffLogRepository;
    }
    
    public async Task LogActionAsync(string action, Staff staff)
    {
        var staffLog = new StaffLog(action, staff.FirstName, staff.LastName,
            staff.LicenseNumber,
            staff.Specialization, staff.ContactInfo, staff.DeletionStatus);

        await _staffLogRepository.AddLogAsync(staffLog);
    }
}