using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Specializations.Interfaces;

namespace dddnet8.Infraestructure.UtilsBootstrapper.Specializations;

public class SpecializationsUtils
{
    private ISpecializationRepository _specializationRepository;

    public SpecializationsUtils(ISpecializationRepository specializationRepository)
    {
        _specializationRepository = specializationRepository;
    }

    public async Task InitializeSpecializationsAsync(){
        var specializations = await _specializationRepository.GetAllAsync();

        if (!specializations.Any())
        {
            await SaveSpecialization(CreateSpecialization("Orthopedics", "A Orthopedics specialization",  "Ort0134"));
            await SaveSpecialization(CreateSpecialization("Anaesthetist", "A Anaesthetist specialization", "Anae1234"));
            await SaveSpecialization(CreateSpecialization("Cleaning", "A Cleaning specialization", "cle9821"));
            await SaveSpecialization(CreateSpecialization("Circulating", "A circulating specialization", "circ9876"));
            await SaveSpecialization(CreateSpecialization("Instrumenting", "A instrumenting specialization", "in1876"));
        } 
    }

    private async Task SaveSpecialization(Specialization createSpecialization)
    {
        await _specializationRepository.AddSpecializationAsync(createSpecialization);
    }

    private  Specialization CreateSpecialization(string name, string description, string code){
        return new Specialization(Name.Create(name), Description.Create(description).Value, SpecializationCode.Create(code));
    }

    public async Task<Specialization> GetSpecialization(string specializationName)
    {
        return (await _specializationRepository.GetByNameAsync(specializationName))!;
    }

    public async Task<List<Specialization>> GetAllSpecializations()
    {
        return await _specializationRepository.GetAllAsync();
    }
}