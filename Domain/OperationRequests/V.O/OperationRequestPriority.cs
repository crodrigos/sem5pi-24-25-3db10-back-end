using System.ComponentModel;
 
namespace dddnet8.Domain.OperationRequests;

public enum OperationRequestPriority
{
    [Description("Emergency")]
    Emergency = 0,
    
    [Description("Very Urgent")]
    VeryUrgent = 1,
    
    [Description("Urgent")]
    Urgent = 2,
    
    [Description("Scheduled")]
    Scheduled = 3,
    
    [Description("Elective")]
    Elective = 4
}