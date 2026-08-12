using dddnet8.Domain.Shared;

namespace dddnet8.Domain.OperationRequests
{
    public class OperationRequestCode : ValueObject, IEquatable<OperationRequestCode>
    {
        public string _operationRequestCode { get; private set; }

        private OperationRequestCode(string operationRequestCode)
        {
            // Validação: Deve começar com "OR" e ter 6 caracteres
            if (string.IsNullOrEmpty(operationRequestCode) || 
                !operationRequestCode.StartsWith("OR"))
            {
                throw new ArgumentException("O código da solicitação de operação deve começar com 'OR' e ter 6 caracteres no total.");
            }

            _operationRequestCode = operationRequestCode;
        }

        // Método Factory (Create) para criar uma instância de OperationRequestCode
        public static OperationRequestCode Create(string operationRequestCode)
        {
            return new OperationRequestCode(operationRequestCode);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Retorna o valor atômico para comparar a instância de OperationRequestCode
            yield return _operationRequestCode;
        }

        public bool Equals(OperationRequestCode? other)
        {
            // Verifica se o outro objeto é nulo ou se os códigos são diferentes
            if (other == null)
                return false;

            return _operationRequestCode.Equals(other._operationRequestCode, StringComparison.OrdinalIgnoreCase);
        }

        // Sobrescrita do método Equals padrão para comparar duas instâncias
        public override bool Equals(object? obj)
        {
            if (obj is OperationRequestCode other)
            {
                return Equals(other);
            }

            return false;
        }

        // Sobrescrita do método GetHashCode para garantir consistência com Equals
        public override int GetHashCode()
        {
            return _operationRequestCode.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }
    }
}
