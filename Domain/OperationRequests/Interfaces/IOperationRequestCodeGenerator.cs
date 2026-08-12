namespace dddnet8.Domain.OperationRequests;

public interface IOperationRequestCodeGenerator
{
    Task<OperationRequestCode> GenerateOperationRequestCode();
}