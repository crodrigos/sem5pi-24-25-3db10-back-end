using System;
using Azure;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Shared;

namespace dddnet8.Infraestructure.OperationTypes;

public interface IOperationTypeRepository : IRepository<OperationType, Guid>
{
    Task<List<OperationType>> GetAllOperationTypesAsync();
    Task<List<OperationType>> GetByStatusAsync(Status status);
    Task<OperationType> UpdateAsync(OperationType operationType);
    Task<OperationType> AddOperationType(OperationType operationType);

    Task<OperationTypeCode?> GetLastOperationTypeCode();

    int Size();
    Task<OperationType?> GetByOperationTypeCode(OperationTypeCode dtoOperationTypeId);
}
