using dddnet8.Domain.Appointments.V.O;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.Appointments.Converter;

public class AppointmentIdConverter : ValueConverter<AppointmentId, string>
{
    public AppointmentIdConverter()
        : base(
            appointmentId => appointmentId.Value, // Converte AppointmentId para string
            str => new AppointmentId(str) // Converte string para AppointmentId
        )
    {
    }
}