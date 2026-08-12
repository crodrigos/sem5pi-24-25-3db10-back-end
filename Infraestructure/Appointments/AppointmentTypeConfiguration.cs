using dddnet8.Domain.Appointments.Converter;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Appointments.V.O;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.SurgeryRooms.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.Appointments;

public class AppointmentTypeConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        
        builder.HasKey(a => a.Id);

        builder.Property(a => a.SurgeryRoom).IsRequired().HasConversion(new RoomNumberConverter());
        
        builder.Property(e => e.OperationRequest).IsRequired().HasConversion(
            v => v._operationRequestCode, 
            v => OperationRequestCode.Create(v));

        builder.Property(a => a.AppointmentDate).IsRequired();

        builder.HasIndex(a => a.OperationRequest).IsUnique();

    }
}