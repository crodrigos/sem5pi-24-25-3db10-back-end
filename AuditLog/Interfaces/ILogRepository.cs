

public interface ILogRepository<T>{
    Task AddLogAsync(T logEntry);
}