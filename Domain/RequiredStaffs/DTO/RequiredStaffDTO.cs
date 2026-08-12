using System;
using dddnet8.Domain.Specializations.DTO;

namespace dddnet8.Domain.RequiredStaffs.DTO;

public class RequiredStaffDto
{
    public string Id { get; set; }
    public string SpecializationName { get; set; }
    public string SpecializationDescription { get; set; }
    
    public string SpecializationCode { get; set; }
    public int Quantity { get; set; }
}
