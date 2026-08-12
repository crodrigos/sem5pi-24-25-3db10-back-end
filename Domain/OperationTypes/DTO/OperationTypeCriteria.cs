using dddnet8.Domain.RequiredStaffs.DTO;

namespace dddnet8.Domain.OperationTypes.DTO;

public class OperationTypeCriteria
{
    public int Id { get; set; }
    public string Name { get; set; }
   
    public int Status { get; set; }
    public int EstimatedDuration { get; set; }
    public List<RequiredStaffCriteria> RequiredStaff { get; set; }
}
