using dddnet8.Domain.Appointments.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.Appointments.Converter;

public class AppointmentStatusConverter : ValueConverter<AppointmentStatus, string>
{
    public AppointmentStatusConverter() : base(
        status => status.ToString(), 
        value => Enum.Parse<AppointmentStatus>(value)) 
    {
    }
}