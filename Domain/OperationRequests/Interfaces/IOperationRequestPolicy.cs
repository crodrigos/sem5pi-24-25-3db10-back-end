using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public interface IOperationRequestPolicy
{
    Task<bool> CanCreateRequest(MedicalRecordNumber patientId, LicenseNumber doctorId);
}