using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public class CreateOperationRequestDto
{
    public string PatientId { get; set; }
    public string DoctorId { get; set; }
    public string OperationTypeId { get; set; }
    public DateTime DeadlineDate { get; set; }
    public string Priority { get; set; }
    public string Description { get; set; }
}