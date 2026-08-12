using dddnet8.Domain.RequiredStaffs.DTO;

namespace dddnet8.Domain.OperationTypes.DTO;

public class OperationTypeAddDTO
{
    public string Name { get; set; }
    public int Status { get; set; }
    public int EstimatedDuration { get; set; }
    public List<RequiredStaffAddDTO> RequiredStaff { get; set; }
}