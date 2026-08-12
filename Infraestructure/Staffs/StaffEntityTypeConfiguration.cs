using App.Domain.SystemUser;
using dddnet8.Domain.Staffs.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;

namespace dddnet8.Infraestructure.Staffs;

public class StaffEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Staffs.Staff>
{
    public void Configure(EntityTypeBuilder<Domain.Staffs.Staff> builder)
    {
        builder.ToTable("Staff");

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

        // Consolidate DeletionStatus configuration into a single line
        builder.Property(u => u.DeletionStatus)
            .IsRequired()
            .HasColumnName("DeletionStatus")
            .HasConversion(new DeletionStatusConverter());
        
        // Configure CreatedOn with default value or behavior
        builder.Property(u => u.CreatedOn)
            .IsRequired()
            .HasColumnName("CreatedOn")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(p => p.LicenseNumber).IsUnique();

        builder.HasIndex(p => p.ContactInfo).IsUnique();
    }
}