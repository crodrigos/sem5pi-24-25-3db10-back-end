using dddnet8.Domain.Shared;

namespace dddnet8.Domain.SurgeryRooms.V.O
{
    public class RoomCapacity : ValueObject
    {
        public int Value { get; private set; }

        public RoomCapacity(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Room capacity must be greater than zero.");

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}