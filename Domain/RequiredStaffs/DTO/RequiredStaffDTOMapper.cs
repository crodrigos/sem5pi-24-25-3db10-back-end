using System;
using dddnet8.Domain.RequiredStaffs;

namespace dddnet8.Domain.RequiredStaffs.DTO;

public class RequiredStaffDTOMapper
{
    public static RequiredStaffDto ToDTO(RequiredStaff requiredStaff) {
        return new RequiredStaffDto {
            Id = requiredStaff.Id.ToString(),
            SpecializationName = requiredStaff.specialization.Name.Value,
            SpecializationDescription = requiredStaff.specialization.Description.Value,
            SpecializationCode = requiredStaff.specialization.Code.Code,
            Quantity = requiredStaff.quantity.Value,
        };
    }
}
