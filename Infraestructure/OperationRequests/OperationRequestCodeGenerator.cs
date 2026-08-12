using dddnet8.Domain.OperationRequests;

namespace dddnet8.Infraestructure.OperationRequests;

public class OperationRequestCodeGenerator : IOperationRequestCodeGenerator
{
    
    private static IOperationRequestRepository _operationRequestRepository;
    
    public OperationRequestCodeGenerator(IOperationRequestRepository operationRequestRepository)
    {
        _operationRequestRepository = operationRequestRepository;
    }
    
    public async Task<OperationRequestCode> GenerateOperationRequestCode()
    {
        var count = await _operationRequestRepository.GetOperationRequestCount();

        var nextCode = $"OR{(count + 1):D4}";

        return OperationRequestCode.Create(nextCode);

    }
}