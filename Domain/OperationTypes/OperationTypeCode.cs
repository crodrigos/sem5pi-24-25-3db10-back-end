using dddnet8.Domain.Shared;
using System;
using System.Collections.Generic;

namespace dddnet8.Domain.OperationTypes
{
    public class OperationTypeCode : ValueObject, IEquatable<OperationTypeCode>
    {
        public string _OperationTypeCode { get; private set; }

        private OperationTypeCode(string operationTypeCode)
        {
            
            if (string.IsNullOrEmpty(operationTypeCode))
            {
                throw new ArgumentException("O código da sala de cirurgia deve começar com 'OT' e ter 6 caracteres no total.");
            }

            _OperationTypeCode = operationTypeCode;
        }

        // Método Factory (Create) para criar uma instância de SurgeryRoomCode
        public static OperationTypeCode Create(string surgeryRoomCode)
        {
            return new OperationTypeCode(surgeryRoomCode);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Retorna o valor atômico para comparar a instância de SurgeryRoomCode
            yield return _OperationTypeCode;
        }

        public bool Equals(OperationTypeCode? other)
        {
            // Verifica se o outro objeto é nulo ou se os códigos são diferentes
            if (other == null)
                return false;

            return _OperationTypeCode.Equals(other._OperationTypeCode, StringComparison.OrdinalIgnoreCase);
        }

        // Sobrescrita do método Equals padrão para comparar duas instâncias
        public override bool Equals(object? obj)
        {
            if (obj is OperationTypeCode other)
            {
                return Equals(other);
            }

            return false;
        }

        // Sobrescrita do método GetHashCode para garantir consistência com Equals
        public override int GetHashCode()
        {
            return _OperationTypeCode.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }
    }
}
