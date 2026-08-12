using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnet8.Infraestructure.SurgeryRooms.TypeConfiguration;

public class SurgeryRoomTypeConfiguration : IEntityTypeConfiguration<SurgeryRoom>
{
    public void Configure(EntityTypeBuilder<SurgeryRoom> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.RoomNumber)
            .HasConversion(new RoomNumberConverter()) 
            .IsRequired();

        builder.Property(s => s.RoomType)
            .HasConversion(new RoomTypeConverter()) 
            .IsRequired();

        builder.Property(s => s.RoomCapacity)
            .HasConversion(new RoomCapacityConverter()) 
            .IsRequired();

        builder.Property(s => s.RoomStatus)
            .HasConversion(new RoomStatusConverter()) 
            .IsRequired();
    }
    
}