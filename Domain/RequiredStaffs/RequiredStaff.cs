using System;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Domain.RequiredStaffs;

[Owned]
public class RequiredStaff : Entity<Guid> {

    public Specialization specialization { get; set; }
    public RequiredStaffQuantity quantity { get; set; }
    public OperationType operationType { get; set; }

    public RequiredStaff(Guid guid, Specialization specialization, RequiredStaffQuantity quantity, OperationType operationType) : base(guid)
    {

        if (specialization == null) {
            throw new ArgumentNullException(nameof(specialization));
        }

        if (operationType == null) {
            throw new ArgumentNullException(nameof(operationType));
        }

        if (quantity == null) {
            throw new ArgumentNullException(nameof(quantity));
        }
    
        this.specialization = specialization;
        this.quantity = quantity;
        this.operationType = operationType;
    }
    
    protected RequiredStaff() : base(Guid.NewGuid())
    {}
}
