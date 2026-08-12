namespace dddnet8.Domain.PlanningModuleNotifications.DTOs;

public class PlanningSurgeryIdDTO
{
    /// <summary>
    /// The code associated with the surgery request.
    /// </summary>
    public string OpRequestCode { get; set; }

    /// <summary>
    /// The type of the surgery request.
    /// </summary>
    public string OpTypeCode { get; set; }

    public PlanningSurgeryIdDTO(string opRequestCode, string opTypeCode)
    {
        OpRequestCode = opRequestCode;
        OpTypeCode = opTypeCode;
    }

    public PlanningSurgeryIdDTO()
    {
    }
}
