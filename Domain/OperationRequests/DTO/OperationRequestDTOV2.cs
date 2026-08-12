using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public class OperationRequestDTOV2
{
    public string OperationRequestCodeId { get; set; } // Ensure the Id is included in the DTO
    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public string OperationTypeId { get; set; }
    public DateTime DeadlineDate { get; set; }
    public string Priority { get; set; }
    public string OperationDescription { get; set; } 
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public bool IsScheduled { get; set; } 
}