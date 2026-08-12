using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Staffs.Converter;

namespace dddnet8.Infrastructure.Configurations
{
    public class AssignedStaffConfiguration : IEntityTypeConfiguration<AssignedStaff>
    {
        public void Configure(EntityTypeBuilder<AssignedStaff> builder)
        {
            // Define o nome da tabela no banco de dados
            builder.ToTable("AssignedStaffs");

            // Define a chave primária (ID)
            builder.HasKey(a => a.Id);

            // Mapeia a propriedade de LicenseNumber
            builder.Property(a => a.AssignedLicenseNumber).HasConversion(new LicenseNumberConverter())
                .IsRequired(); // Torna a propriedade obrigatória
                

            // Mapeia a propriedade de AppointmentId (relacionamento com Appointment)
            builder.Property(a => a.AppointmentId)
                .IsRequired(); // Torna a propriedade obrigatória

            // Relacionamento entre AssignedStaff e Appointment
            builder.HasOne(a => a.Appointment)
                .WithMany()  // Sem necessidade de coleção em Appointment, mas se tiver, use .WithMany(x => x.AssignedStaffs)
                .HasForeignKey(a => a.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade); // Especifica o comportamento de exclusão (CASCADE significa que se o Appointment for deletado, os AssignedStaffs relacionados também serão deletados)

           

            builder.Property(a => a.AssignedLicenseNumber).IsRequired(); 

        }
    }
}