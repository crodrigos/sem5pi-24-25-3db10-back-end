using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Appointments.V.O;
public class AppointmentId : ValueObject
    {
        public string Value { get; private set; }

        // Construtor
        public AppointmentId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Appointment ID cannot be empty or null.");

            Value = value;
        }

        // Método para comparar dois AppointmentId's
        protected bool Equals(ValueObject other)
        {
            if (other is AppointmentId otherAppointmentId)
            {
                return Value.Equals(otherAppointmentId.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Método para calcular o hash
        protected int GetHashCodeCore()
        {
            return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        // Método para extrair os valores atômicos do ValueObject
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }

