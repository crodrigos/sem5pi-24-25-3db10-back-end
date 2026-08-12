using System.ComponentModel;

namespace dddnet8.Domain.OperationRequests;

public enum OperationRequestStatus
{
    [Description("Pending")]
    Pending,
    
    [Description("Approved")]
    Approved,
    
    [Description("Rejected")]
    Rejected,
    
    [Description("Scheduled")]
    Scheduled,
    
    [Description("Completed")]
    Completed,
    
    [Description("Cancelled")]
    Cancelled
}