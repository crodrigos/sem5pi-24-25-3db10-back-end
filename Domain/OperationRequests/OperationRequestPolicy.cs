using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public class OperationRequestPolicy : IOperationRequestPolicy
{
    private static readonly TimeSpan AllowedTimeFrame = TimeSpan.FromMinutes(5);
    private readonly IOperationRequestRepository _operationRequestRepository;
    
    public OperationRequestPolicy(IOperationRequestRepository operationRequestRepository)
    {
        _operationRequestRepository = operationRequestRepository;
    }
    
    public async Task<bool> CanCreateRequest(MedicalRecordNumber patientId, LicenseNumber doctorId)
    {
            var since = DateTime.UtcNow - AllowedTimeFrame;
            return !await _operationRequestRepository.ExistsRecentRequest(patientId, doctorId, since);
    }
}