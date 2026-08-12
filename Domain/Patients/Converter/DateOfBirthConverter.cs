using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SurgicalManagement.Domain.Domain;

namespace YourNamespace.Converters
{
    public class DateOfBirthConverter : ValueConverter<DateOfBirth, DateTime>
    {
        public DateOfBirthConverter() 
            : base(
                dob => dob.Value,                  // Converte de DateOfBirth para DateTime
                dt =>  DateOfBirth.Create(dt)         // Converte de DateTime para DateOfBirth
            )
        {
        }
    }
}