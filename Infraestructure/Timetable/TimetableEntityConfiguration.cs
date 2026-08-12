using dddnet8.Domain.Staffs.Converter;
using dddnet8.Domain.Staffs.V.O;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.Timetable;

public class TimetableEntityConfiguration : IEntityTypeConfiguration<Domain.Timetables.Timetable>
{
    public void Configure(EntityTypeBuilder<Domain.Timetables.Timetable> builder)
    {
        
        builder.ToTable("Timetables");

        builder.HasKey(u => u.Id);

        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasConversion(new LicenseNumberConverter());

        builder.Property(x => x.DateShift)
            .IsRequired();
            

        builder.Property(x => x.TimeShift)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => TimeShift.FromString(v) 
            );
    }
}