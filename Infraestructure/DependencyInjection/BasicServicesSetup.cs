using dddnet8.Domain.Shared;

namespace dddnet8.Infraestructure.DependencyInjection;

public class BasicServicesSetup : ISetupServices
{
    public void Setup(IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}