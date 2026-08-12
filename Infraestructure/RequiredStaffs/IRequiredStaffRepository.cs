using System;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Infraestructure.RequiredStaffs;

public interface IRequiredStaffRepository : IRepository<RequiredStaff, Guid>
{
    Task<List<RequiredStaff>> GetByOperationTypeAsync(OperationType operationType);
    Task<bool> RemoveByOperationType(OperationType operationType);
    Task<bool> Save();
    Task<List<OperationTypeCode>> GetOperationTypesBySpecialization(Specialization specialization);
}
