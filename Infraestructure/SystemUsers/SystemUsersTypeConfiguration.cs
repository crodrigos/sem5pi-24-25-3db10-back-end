using App.Domain.SystemUser;
using dddnet8.Domain.SystemUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.SystemUsers
{
    internal class SystemUsersTypeConfiguration : IEntityTypeConfiguration<SystemUser>
    {
        public void Configure(EntityTypeBuilder<SystemUser> builder)
        {
          
                builder.ToTable("SystemUsers");
            
                builder.HasKey(u => u.Id);


                builder.Property(u => u.EmailAddress)
                    .HasConversion(new EmailAddressConverter())
                    .IsRequired();
            
                builder.Property(u => u.Username)
                    .HasConversion(new EmailAddressConverter())
                    .IsRequired();
            
                builder.Property(u => u.Role)
                    .HasConversion<string>()
                    .IsRequired();

                builder.Property(u => u.Password)
                    .IsRequired();
            
                builder.Property(u => u.CreatedOn)
                    .IsRequired();

                builder.Property(u => u.DeletionStatus)
                    .HasColumnName("DeletionStatus").HasConversion(new DeletionStatusConverter());
            
            
                builder.HasIndex(u => u.EmailAddress).IsUnique();
                builder.HasIndex(u => u.Username).IsUnique();
            
            
        }
    }    
}
