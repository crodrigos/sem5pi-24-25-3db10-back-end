using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public interface IOperationRequestRepository : IRepository<OperationRequest, Guid>
{
    Task AddOperationRequestAsync(OperationRequest operationRequest);
    Task<IEnumerable<OperationRequest>> SearchOperationRequestsByFiltersAsync(OperationRequestCriteria criteria);
    Task RemoveOperationRequest(OperationRequest operationRequest);
    Task<bool> ExistsRecentRequest(MedicalRecordNumber patientId, LicenseNumber doctorId, DateTime since);
    Task<int> GetOperationRequestCount();
    Task<OperationRequest?> GetByOperationRequestCode(string operationRequestCode);
}