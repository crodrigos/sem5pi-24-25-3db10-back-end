using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.Staffs.Converter;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.RequiredStaffs;

public class RequiredStaffEntityConfiguration : IEntityTypeConfiguration<RequiredStaff>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RequiredStaff> builder)
    {
        builder.ToTable("RequiredStaffs");

            builder.HasKey(rs => rs.Id);

            builder.Property(rs => rs.specialization)
                .IsRequired()
                .HasConversion(new SpecializationConverter());


            builder.Property(rs => rs.quantity)
                .IsRequired()
                .HasConversion(
                    v => v.Value,
                    v => new RequiredStaffQuantity(v)
                );

            builder.HasOne(rs => rs.operationType)
                .WithMany()
                .HasForeignKey("OperationTypeId")
                .IsRequired();

            // Additional configurations if needed
    }
}