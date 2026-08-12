using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public class OperationRequestDto
{
    public Guid Id { get; set; } // Ensure the Id is included in the DTO
    public MedicalRecordNumber PatientId { get; set; }
    public LicenseNumber DoctorId { get; set; }
    public OperationTypeCode OperationTypeId { get; set; }
    public DateTime DeadlineDate { get; set; }
    public OperationRequestPriority Priority { get; set; }
    public string OperationDescription { get; set; } 
    public OperationRequestStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public bool IsScheduled { get; set; } 
}

public class GetAllOperationRequestsDto
{
    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public string OperationTypeId { get; set; }
    public string OperationRequestCode { get; set; }
}

public class GetAllOperationRequestForAppointmentDTO
{
    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public string OperationTypeId { get; set; }
    public string OperationRequestCode { get; set; }
    public string OperationRequestDescription { get; set; }
}