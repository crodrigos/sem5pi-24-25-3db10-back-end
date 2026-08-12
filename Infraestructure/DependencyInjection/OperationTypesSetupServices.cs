using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.RequiredStaffs;

namespace dddnet8.Infraestructure.DependencyInjection;

public class OperationTypesSetupServices : ISetupServices
{
    public void Setup(IServiceCollection services)
    {
        services.AddSingleton<IOperationTypeService, OperationTypeService>();
        services.AddSingleton<IOperationTypeRepository, OperationTypeRepository>();
        services.AddSingleton<IRequiredStaffRepository, RequiredStaffRepository>();
    }
}