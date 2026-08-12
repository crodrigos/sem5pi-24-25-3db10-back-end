using System;

namespace SurgicalManagement.Domain.Domain
{
    public class DateOfBirth : IEquatable<DateOfBirth>
    {
        private readonly DateTime _date; // O valor da data de nascimento

        // Construtor privado
        private DateOfBirth(DateTime date)
        {
            _date = date;
        }

        // Método de fábrica para criar uma nova instância de DateOfBirth
        public static DateOfBirth Create(DateTime date)
        {
            ValidateDate(date); // Validação da data
            return new DateOfBirth(date);
        }
        
        public DateOfBirth(){}

        // Validações
        private static void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentException("Date of birth cannot be in the future.");
            }

            if (date.Year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid Date. Please enter a valid year.");
            }

            // Verifica o mês
            if (date.Month < 1 || date.Month > 12)
            {
                throw new ArgumentException("Invalid Month. Please enter a valid month.");
            }

            // Verifica o dia em meses específicos
            if ((date.Month == 1 || date.Month == 3 || date.Month == 5 || date.Month == 7 || date.Month == 8 || date.Month == 10 || date.Month == 12) && (date.Day < 1 || date.Day > 31))
            {
                throw new ArgumentException($"Invalid Date. Please enter a valid date of birth. Month {date.Month} has 31 days.");
            }

            if ((date.Month == 4 || date.Month == 6 || date.Month == 9 || date.Month == 11) && (date.Day < 1 || date.Day > 30))
            {
                throw new ArgumentException($"Invalid Date. Please enter a valid date of birth. Month {date.Month} has 30 days.");
            }

            if (date.Month == 2)
            {
                if (date.Day > 29)
                {
                    throw new ArgumentException("February has a maximum of 29 days.");
                }
                if (date.Day == 29 && !DateTime.IsLeapYear(date.Year))
                {
                    throw new ArgumentException("The year is not a leap year. February can have a maximum of 28 days.");
                }
            }
        }

        // Propriedade para acessar o valor da data
        public DateTime Value => _date;

        // Implementação do método Equals
        public bool Equals(DateOfBirth? other)
        {
            if (other is null) return false;
            return _date == other._date;
        }

        // Sobrescrevendo ToString para facilitar a visualização
        public override string ToString()
        {
            int age = CalculateAge(); // Calcula a idade
            return $"{_date:MMMM d, yyyy} (Age: {age})"; // Formato desejado
        }

        private int CalculateAge()
        {
            int age = DateTime.Now.Year - _date.Year;
            if (DateTime.Now < _date.AddYears(age)) age--; // Ajusta a idade se ainda não tiver passado o aniversário
            return age;
        }
        public string ToDatabaseFormat()
        {
            return _date.ToString("yyyy-MM-dd"); // Formato ISO 8601
        }
    }
}
