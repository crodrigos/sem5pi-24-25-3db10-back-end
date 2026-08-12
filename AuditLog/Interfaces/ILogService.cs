namespace dddnet8.AuditLog.Interfaces;

public interface ILogService<T>
{
    Task LogActionAsync(string action, T entity);
}