namespace dddnet8.Domain.Shared
{
    public class Coordinate : ValueObject, IEquatable<Coordinate>
    {
        private readonly int _x;
        private readonly int _y;

        public int X => _x;
        public int Y => _y;

        private Coordinate(int x, int y)
        {
            _x = x;
            _y = y;
        }

        // Método Create
        public static Coordinate Create(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException("Coordinates cannot be negative.");
            }

            return new Coordinate(x, y);
        }

        // Método FromString
        public static Coordinate FromString(string coordinateString)
        {
            if (string.IsNullOrWhiteSpace(coordinateString))
            {
                throw new ArgumentException("Coordinate string cannot be null or empty.");
            }

            // Remove os parênteses e divide a string em X e Y
            var cleanedString = coordinateString.Trim('(', ')');
            var parts = cleanedString.Split('-');

            if (parts.Length != 2)
            {
                throw new ArgumentException("Coordinate string must be in the format '(X - Y)'.");
            }

            // Tenta converter as partes para inteiros
            if (int.TryParse(parts[0].Trim(), out int x) && int.TryParse(parts[1].Trim(), out int y))
            {
                return Create(x, y);
            }
            else
            {
                throw new ArgumentException("Invalid coordinate format. Both X and Y should be valid integers.");
            }
        }

        protected Coordinate() {}

        public override bool Equals(object? obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public bool Equals(Coordinate? other)
        {
            if (other is null) return false;
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _x;
            yield return _y;
        }

        public override string ToString()
        {
            return $"({X} - {Y})"; // Simplificado, sem a necessidade de ToString() explícito
        }

        public static explicit operator string(Coordinate coordinate) => coordinate.ToString();

        public static explicit operator Coordinate((int x, int y) value) => Create(value.x, value.y);
    }
}
