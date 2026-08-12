using System;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.RequiredStaffs.DTO;

namespace dddnet8.Domain.OperationTypes.DTO;

public class OperationTypeDTOMapper {

    public static OperationTypeDTO ToDTO(OperationType operationType, List<RequiredStaff> requiredStaff) {

        // TODO -> MELHORAR ISTO RODRI
        List<RequiredStaffDto> requiredStaffDTO = requiredStaff.Select(RequiredStaffDTOMapper.ToDTO).ToList();

        return new OperationTypeDTO {
            Id = operationType.Id.ToString(),
            Code = operationType.OperationTypeCode._OperationTypeCode,
            Name = operationType.Name.Value,
            Status = (int) operationType.Status,
            EstimatedDuration = Convert.ToInt32(operationType.EstimatedDuration.SurgeryDuration.TotalMinutes),
            RequiredStaff = requiredStaffDTO
        };
    }
}
