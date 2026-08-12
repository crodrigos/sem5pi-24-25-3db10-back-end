using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.Converter;
using dddnet8.Domain.Staffs.V.O;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.Staff;

public class TimeSlotTypeConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.ToTable("TimeSlots");

        builder.HasKey(u => u.Id);

        builder.Property(p => p.Date).IsRequired();

        builder.Property(p => p.LicenseNumber)
            .IsRequired()
            .HasConversion(new LicenseNumberConverter());
        

        builder.Property(x => x.TimeShift)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => TimeShift.FromString(v) 
            );

    }
}