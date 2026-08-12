using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SurgicalManagement.Domain.Common;

public class DeletionStatusConverter : ValueConverter<DeletionStatus, string>
{
    public DeletionStatusConverter()
        : base( d => d.ToString(),
            d=> DeletionStatus.FromString(d)
        )
    {
    }
}