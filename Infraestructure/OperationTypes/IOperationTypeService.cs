using System;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.DTO;

namespace dddnet8.Infraestructure.OperationTypes;

public interface IOperationTypeService 
{
    Task<List<OperationTypeDTO>> GetAll();
    Task<List<OperationTypeDTO>> GetByStatus(int status);
    Task<OperationTypeDTO> GetById(string id);  
    Task<OperationTypeDTO> Add(OperationTypeAddDTO operationTypeDTO);
    Task<OperationTypeDTO> Update(string id, OperationTypeDTO operationTypeDTO);
    Task<OperationTypeDTO> RemoveByCode(string code);
}
 