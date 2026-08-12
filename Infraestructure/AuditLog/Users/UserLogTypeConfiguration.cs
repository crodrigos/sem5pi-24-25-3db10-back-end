using App.Domain.SystemUser;
using dddnet8.AuditLog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.AuditLog.Users;

public class UserLogTypeConfiguration : IEntityTypeConfiguration<UserLog>
{
    public void Configure(EntityTypeBuilder<UserLog> builder)
    {
        builder.ToTable("UserLog");

        builder.HasKey(u => u.Id);
        
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Username)
            .HasConversion(new EmailAddressConverter())
            .IsRequired();

        builder.Property(p => p.Password)
            .IsRequired();

        builder.Property(p => p.EmailAddress)
            .HasConversion(new EmailAddressConverter())
            .IsRequired();

        builder.Property(p => p.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.CreatedOn)
            .IsRequired(); ;
    }
}