using dddnet8.Domain.OperationTypes;

namespace dddnet8.Infraestructure.OperationTypes;

public class OperationTypeCodeGenerator : IOperationTypeCodeGenerator
{
    
    private static IOperationTypeRepository _operationTypeCodeGenerator;

    public OperationTypeCodeGenerator(IOperationTypeRepository operationTypeRepository)
    {
        _operationTypeCodeGenerator = operationTypeRepository;
    }

    public OperationTypeCode GenerateOperationCode()
    {
        // Obtém o número total de tipos de operação registrados na base de dados
        var count = _operationTypeCodeGenerator.Size();

        var nextCode = $"OT{(count + 1):D4}"; 

        // Retorna o código formatado com 4 dígitos
        return OperationTypeCode.Create(nextCode);
    }

}