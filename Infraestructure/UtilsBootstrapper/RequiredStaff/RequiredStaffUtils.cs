using dddnet8.Infraestructure.RequiredStaffs;
using dddnet8.Infraestructure.UtilsBootstrapper.OperationTypes;
using dddnet8.Infraestructure.UtilsBootstrapper.Specializations;
using dddnet8.Domain.RequiredStaffs;

namespace dddnet8.Infraestructure.UtilsBootstrapper.RequiredStaff;

public class RequiredStaffUtils
{
    private readonly SpecializationsUtils _specializationsUtils;
    private readonly OperationTypeUtils _operationTypeUtils;
    private readonly IRequiredStaffRepository _requiredStaffRepository;

    public RequiredStaffUtils(SpecializationsUtils specializationsUtils, OperationTypeUtils operationTypeUtils, IRequiredStaffRepository requiredStaffRepository)
    {
        _specializationsUtils = specializationsUtils;
        _operationTypeUtils = operationTypeUtils;
        _requiredStaffRepository = requiredStaffRepository;
    }
    
    
    public async Task InitializeRequiredStaffAsync(){
        var requiredStaffs = await _requiredStaffRepository.GetAllAsync();

        if (!requiredStaffs.Any())
        {
            await SaveRequiredStaff(await CreateRequiredStaff(1));
        } 
    }

    private async Task SaveRequiredStaff(List<Domain.RequiredStaffs.RequiredStaff> createRequiredStaff)
    {
        foreach (var requiredStaff in createRequiredStaff)
        {
            await _requiredStaffRepository.AddAsync(requiredStaff);
        }
    }

    private async Task<List<Domain.RequiredStaffs.RequiredStaff>> CreateRequiredStaff(int requiredQuantity){
        var opCodes = await _operationTypeUtils.GetAllOperationTypes();
        
        var allSpecializations = await _specializationsUtils.GetAllSpecializations();
        
        var quantity = new RequiredStaffQuantity(requiredQuantity);

        List<Domain.RequiredStaffs.RequiredStaff> requiredStaffs = new List<Domain.RequiredStaffs.RequiredStaff>();
        
        foreach (var opCode in opCodes) {
            
            foreach (var s in allSpecializations){
                
                Domain.RequiredStaffs.RequiredStaff rs = new Domain.RequiredStaffs.RequiredStaff(Guid.NewGuid(), s, quantity, opCode);
                
                requiredStaffs.Add(rs);
            }
        }

        return requiredStaffs;
    }
}