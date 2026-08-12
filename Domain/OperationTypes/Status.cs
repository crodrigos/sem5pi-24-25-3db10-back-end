using System.ComponentModel;

namespace dddnet8.Domain.OperationTypes;

public enum Status : int
{
    [Description("Active")]
    Active = 1,
    [Description("Inactive")]
    Inactive = 0
}