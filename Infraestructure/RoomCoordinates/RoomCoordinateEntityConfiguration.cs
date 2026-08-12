using App.Domain.SystemUser;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.RoomCoordinates.Converter;
using dddnet8.Domain.RoomCoordinates.Domain;
using dddnet8.Domain.SurgeryRooms.Converter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourNamespace.Converters;

namespace dddnet8.Infraestructure.RoomCoordinates;

public class RoomCoordinateEntityConfiguration : IEntityTypeConfiguration<RoomCoordinate>
{
    public void Configure(EntityTypeBuilder<RoomCoordinate> builder)
    {
        builder.ToTable("RoomCoordinates");
                
        builder.HasKey(u => u.Id);
        
        builder.Property(p => p.RoomNumber).IsRequired().HasConversion(new RoomNumberConverter());

        builder.Property(p => p.DoorDirection).IsRequired().HasConversion(new DoorDirectionConverter());
                
        builder.Property(p => p.Position).IsRequired().HasConversion(new CoordinateConverter());
                
        builder.Property(p => p.Size).IsRequired().HasConversion(new DimensionConverter());

        builder.Property(p => p.CreatedOn);

    }
}