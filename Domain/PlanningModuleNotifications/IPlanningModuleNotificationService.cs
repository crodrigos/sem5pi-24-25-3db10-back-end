namespace dddnet8.Domain.PlanningModuleNotifications;

public interface IPlanningModuleNotificationService
{
    Task NotifyAsync(string message);
}