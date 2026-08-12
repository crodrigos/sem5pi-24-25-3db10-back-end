using dddnet8.Domain.OperationTypes;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.UtilsBootstrapper.Specializations;

namespace dddnet8.Infraestructure.UtilsBootstrapper.OperationTypes;

public class OperationTypeUtils
{
    private IOperationTypeRepository _operationTypeRepository;
    private readonly SpecializationsUtils _specializationsUtils;

    public OperationTypeUtils(IOperationTypeRepository operationTypeRepository, SpecializationsUtils specializationsUtils)
    {
        _operationTypeRepository = operationTypeRepository;
        _specializationsUtils = specializationsUtils;
        
    }
    
    public async Task InitializeOperationTypesAsync(){
        var operationTypes = await _operationTypeRepository.GetAllAsync();

        if (!operationTypes.Any())
        {
            await SaveOperationType(await CreateOperationType(Guid.NewGuid(),"Knee Replacement Surgery", 45,60,45, "OT0001"));
            await SaveOperationType(await CreateOperationType(Guid.NewGuid(),"Shoulder Replacement Surgery", 45,90,45, "OT0002"));
            await SaveOperationType(await CreateOperationType(Guid.NewGuid(),"Hip Replacement Surgery", 45,75,45, "OT0003"));
        } 
    }

    private async Task SaveOperationType(OperationType createOperationType)
    {
      await _operationTypeRepository.AddOperationType(createOperationType);
    }

    private async Task<OperationType> CreateOperationType(Guid newGuid, string SurgeryName, int anesthesiaTime, int surgeryTime, int cleaningTime, string opTypeCode)
    {
        Domain.OperationTypes.Names.Name name = new Domain.OperationTypes.Names.Name(SurgeryName);
        EstimatedDuration duration = new EstimatedDuration(TimeSpan.FromMinutes(anesthesiaTime), TimeSpan.FromMinutes(surgeryTime), TimeSpan.FromMinutes(cleaningTime));
        OperationTypeCode operationTypeCode = OperationTypeCode.Create(opTypeCode);
        var specialization = await _specializationsUtils.GetSpecialization("Orthopedics");
        
        return new OperationType(newGuid, name, Status.Active, duration, operationTypeCode){
            SpecializationRequired = specialization
        };
    }
    
    public async Task<OperationType> GetOperationType(string operationCode)
    {
        return (await _operationTypeRepository.GetByOperationTypeCode(OperationTypeCode.Create(operationCode)))!;
    }

    public async Task<List<OperationType>> GetAllOperationTypes()
    {
        return await _operationTypeRepository.GetAllAsync();
    }
    
}