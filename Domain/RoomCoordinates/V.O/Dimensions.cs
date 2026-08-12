namespace dddnet8.Domain.Shared
{
    public class Dimensions : ValueObject, IEquatable<Dimensions>
    {
        private readonly int _width;
        private readonly int _length;

        public int Width => _width;
        public int Length => _length;

        private Dimensions(int width, int length)
        {
            if (width <= 0 || length <= 0)
            {
                throw new ArgumentException("Width and Length must be positive integers.");
            }

            _width = width;
            _length = length;
        }

        // Método Create
        public static Dimensions Create(int width, int length)
        {
            return new Dimensions(width, length);
        }

        // Método FromString
        public static Dimensions FromString(string dimensionsString)
        {
            if (string.IsNullOrWhiteSpace(dimensionsString))
            {
                throw new ArgumentException("Dimensions string cannot be null or empty.");
            }

            // Divide a string no caractere '-'
            var parts = dimensionsString.Split('-');

            if (parts.Length != 2)
            {
                throw new ArgumentException("Dimensions string must be in the format 'Width-Length'.");
            }

            // Tenta converter as partes para inteiros
            if (int.TryParse(parts[0].Trim(), out int width) && int.TryParse(parts[1].Trim(), out int length))
            {
                return Create(width, length);
            }
            else
            {
                throw new ArgumentException("Invalid dimensions format. Both Width and Length should be valid integers.");
            }
        }

        protected Dimensions() {}

        public override bool Equals(object? obj)
        {
            return obj is Dimensions other && Equals(other);
        }

        public bool Equals(Dimensions? other)
        {
            if (other is null) return false;
            return _width == other._width && _length == other._length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_width, _length);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _width;
            yield return _length;
        }

        public override string ToString()
        {
            return $"{Width}-{Length}";
        }

        public static explicit operator string(Dimensions dimensions) => dimensions.ToString();

        public static explicit operator Dimensions((int width, int length) value) => Create(value.width, value.length);
    }
}
