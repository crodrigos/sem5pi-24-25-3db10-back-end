using dddnet8.Domain.Shared;

namespace dddnet8.Domain.SurgeryRooms.V.O;

public class RoomNumber : ValueObject {
        public string Value { get; private set; }

        public RoomNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Room number cannot be empty or whitespace.");

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Retorna o valor do número da sala para comparação
            yield return Value; 
        } 
}
