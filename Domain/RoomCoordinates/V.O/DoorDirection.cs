namespace dddnet8.Domain.Shared
{
    public class DoorDirection : ValueObject, IEquatable<DoorDirection>
    {
        private readonly int _direction; // Representa a direção como um número inteiro não negativo

        public int Direction => _direction;

        private DoorDirection(int direction)
        {
            if (direction < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(direction), "Direction cannot be less than 0.");
            }

            _direction = direction;
        }

        // Método Create
        public static DoorDirection Create(int direction)
        {
            return new DoorDirection(direction);
        }

        // Método FromString
        public static DoorDirection FromString(string directionString)
        {
            if (string.IsNullOrWhiteSpace(directionString))
            {
                throw new ArgumentException("Direction string cannot be null or empty.", nameof(directionString));
            }

            // Remove espaços e tenta converter para um inteiro
            directionString = directionString.Trim();

            if (int.TryParse(directionString, out int direction))
            {
                if (direction < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(direction), "Direction cannot be less than 0.");
                }
                return Create(direction);
            }
            else
            {
                throw new ArgumentException("Invalid direction format. The direction must be a valid non-negative integer.", nameof(directionString));
            }
        }

        protected DoorDirection() {}

        public override bool Equals(object? obj)
        {
            return obj is DoorDirection other && Equals(other);
        }

        public bool Equals(DoorDirection? other)
        {
            if (other is null) return false;
            return _direction == other._direction;
        }

        public override int GetHashCode()
        {
            return _direction.GetHashCode();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _direction;
        }

        public override string ToString()
        {
            return _direction.ToString();
        }

        public static explicit operator string(DoorDirection doorDirection) => doorDirection.ToString();

        public static explicit operator DoorDirection(int direction) => Create(direction);
    }
}
