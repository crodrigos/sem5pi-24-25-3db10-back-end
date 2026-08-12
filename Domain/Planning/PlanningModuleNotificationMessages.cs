namespace dddnet8.Domain.PlanningModuleNotifications;

public static class PlanningModuleNotificationMessages
{
    public static string OperationRequestDeleted(Guid id) => $"{{\"action\":\"deleted\", \"operationRequestId\":\"{id}\"}}";
    public static string OperationRequestUpdated(string id) => $"{{\"action\":\"updated\", \"operationRequestId\":\"{id}\"}}";
}