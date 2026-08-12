using App.Domain.SystemUser;
using dddnet8.AuditLog.Entities;
using dddnet8.Domain.Staffs.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;
using YourNamespace.GDPR.Entities;

namespace dddnet8.Infraestructure.AuditLog.Staffs;

public class StaffLogTypeConfiguration : IEntityTypeConfiguration<StaffLog>
{
    public void Configure(EntityTypeBuilder<StaffLog> builder)
    {
        builder.ToTable("StaffLog");

        builder.HasKey(u => u.Id);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasConversion(new NameConverter());

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasConversion(new NameConverter());

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasConversion(new NameConverter());

        builder.Property(u => u.LicenseNumber)
            .IsRequired()
            .HasConversion(new LicenseNumberConverter());

        builder.Property(p => p.ContactInfo)
            .IsRequired()
            .HasConversion(new ContactInfoConverter());

        builder.Property(u => u.Specialization)
            .IsRequired()
            .HasConversion(new SpecializationConverter());
        
        builder.Property(p => p.DeletionStatus)
            .HasColumnName("DeletionStatus")
            .HasConversion(new DeletionStatusConverter());
    }
}