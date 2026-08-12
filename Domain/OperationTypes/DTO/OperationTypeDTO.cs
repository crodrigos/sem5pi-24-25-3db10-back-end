using System;
using dddnet8.Domain.RequiredStaffs.DTO;

namespace dddnet8.Domain.OperationTypes.DTO;

public class OperationTypeDTO
{
    public string Id { get; set; }
    public string Code { get; set; }
    public required string Name { get; set; }
    public required int Status { get; set; }
    public required int EstimatedDuration { get; set; }
    public required List<RequiredStaffDto> RequiredStaff { get; set; }
}
