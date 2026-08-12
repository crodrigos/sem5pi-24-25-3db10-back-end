using dddnet8.Domain.Shared;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnet8.Domain.OperationRequests.Converter;

public class DescriptionConverter : ValueConverter<Description, string>
{
    public DescriptionConverter()
        : base(
            v => v.ToString(),
            v => Description.FromString(v).Value
        )
    {
    }
}