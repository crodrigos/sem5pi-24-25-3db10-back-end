using dddnet8.AuditLog.Entities;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.SystemUsers;

namespace dddnet8.AuditLog.Services;

public class UserLogService : ILogService<SystemUser>
{
    private readonly ILogRepository<UserLog> _userLogRepository;

    public UserLogService(ILogRepository<UserLog> userLogRepository)
    {
        _userLogRepository = userLogRepository;
    }
    
    public async Task LogActionAsync(string action, SystemUser user)
    {
        var staffLog = new UserLog(action, "staff", user.Id, user.Username, user.Password, user.CreatedOn, user.Role, user.EmailAddress);

        await _userLogRepository.AddLogAsync(staffLog);
    }
}