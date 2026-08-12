using System;
using dddnet8.Domain.Shared;

namespace dddnet8.Domain.RequiredStaffs;

public class RequiredStaffQuantity : ValueObject
{
    public  int Value { get; private set; }

    public RequiredStaffQuantity(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Required Staff Quantity must be greater than 0");
        }

        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        throw new NotImplementedException();
    }
}
