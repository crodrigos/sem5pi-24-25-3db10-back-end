using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public class OperationRequestCriteria
{
    public string? DoctorId { get; set; }
    public string? PatientId { get; set; }
    public string? OperationTypeId { get; set; }
    public DateTime? Deadline { get; set; }
    
    public string? Priority { get; set; }
    public string? Status { get; set; }

    // Default constructor (necessary for model binding)
    public OperationRequestCriteria()
    {
    }

    // Constructor with parameters for easy instantiation
    public OperationRequestCriteria(
        string? doctorId = null,
        string? patientId = null,
        string? operationTypeId = null,
        DateTime? deadline = null,
        string? priority = null,
        string? status = null)
    {
        DoctorId = doctorId;
        PatientId = patientId;
        OperationTypeId = operationTypeId;
        Deadline = deadline;
        Priority = priority;
        Status = status;
    }
}