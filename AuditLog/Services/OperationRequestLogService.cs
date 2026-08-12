using dddnet8.AuditLog.Entities;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.OperationRequests;

namespace dddnet8.AuditLog.Services;

public class OperationRequestLogService : ILogService<OperationRequest>
{
    private readonly ILogRepository<OperationRequestLog> _operationRequestLogRepository;

    public OperationRequestLogService(ILogRepository<OperationRequestLog> operationRequestLogRepository)
    {
        _operationRequestLogRepository = operationRequestLogRepository;
    }

    public async Task LogActionAsync(string action, OperationRequest operationRequest)
    {
        var operationRequestLog = new OperationRequestLog(action, operationRequest);
        await _operationRequestLogRepository.AddLogAsync(operationRequestLog);
    }
}