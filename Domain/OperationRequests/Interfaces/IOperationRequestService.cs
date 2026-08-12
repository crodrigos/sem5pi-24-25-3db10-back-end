using dddnet8.Infraestructure.Shared;

namespace dddnet8.Domain.OperationRequests;

public interface IOperationRequestService
{
    Task<OperationRequestDto> CreateOperationRequest(CreateOperationRequestDto dto);
    Task<Result<OperationRequestDto>> GetOperationRequest(Guid id);
    Task<Result<List<GetAllOperationRequestsDto>>> GetAllOperationRequests();
    Task<OperationRequestDto> UpdateOperationRequest(OperationRequestCriteria dto, string id);
    Task<Result<string>> DeleteOperationRequest(string id);
    Task<Result<List<OperationRequestDTOV2>>> SearchOperationRequests(OperationRequestCriteria criteria);
}