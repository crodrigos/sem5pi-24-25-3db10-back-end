using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dddnet8.Domain.Patients.VO.Name;

namespace YourNamespace.Converters
{
    public class EmergencyContactConverter : ValueConverter<EmergencyContact, string>
    {
        public EmergencyContactConverter() 
            : base(
                ec =>  ec.ToString(), // Converte de EmergencyContact para string
                str => EmergencyContact.FromString(str) // Converte de string para EmergencyContact
            )
        {
        }
    }
}