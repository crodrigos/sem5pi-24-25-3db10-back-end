using dddnet8.Domain.Shared;


namespace YourNamespace.Domain
{
    public class MedicalCondition : ValueObject
    {
        public string ConditionName { get; private set; }

        // Construtor
        public MedicalCondition(string conditionName)
        {
            if (string.IsNullOrWhiteSpace(conditionName))
                throw new ArgumentException("Condition name cannot be null or empty.", nameof(conditionName));

            ConditionName = conditionName;
        }

        // Implementação de Equals e GetHashCode para comparação de Value Objects
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return ConditionName == ((MedicalCondition)obj).ConditionName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConditionName);
        }

        // Método para retornar os valores que compõem o objeto
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ConditionName;
        }

        // ToString para facilitar a visualização
        public override string ToString()
        {
            return ConditionName;
        }
    }
}