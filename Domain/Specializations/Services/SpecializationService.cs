using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations.DTO;
using dddnet8.Domain.Specializations.Interfaces;

namespace dddnet8.Domain.Specializations.Services;

public class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecializationService"/> class.
    /// </summary>
    /// <param name="specializationRepository">The specialization repository for data operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies are null.</exception>
    public SpecializationService(ISpecializationRepository specializationRepository)
    {
        _specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
    }

    /// <summary>
    /// Creates a new specialization.
    /// </summary>
    /// <param name="createSpecializationDto">The DTO containing specialization information.</param>
    /// <returns>A task representing the asynchronous operation, containing the created specialization as a DTO.</returns>
    public async Task<SpecializationDto> CreateSpecialization(SpecializationDto createSpecializationDto)
    {
        if (createSpecializationDto == null)
            throw new ArgumentNullException(nameof(createSpecializationDto), "Specialization DTO cannot be null.");

        var specialization = new Specialization(
            Name.Create(createSpecializationDto.Name),
            Description.Create(createSpecializationDto.Description).Value,
            SpecializationCode.Create(createSpecializationDto.SpecializationCode)
        );

        await _specializationRepository.AddSpecializationAsync(specialization);

        return SpecializationMapper.ToDto(specialization);
    }

    /// <summary>
    /// Updates an existing specialization.
    /// </summary>
    /// <param name="specializationCode">The name of the specialization to update.</param>
    /// <param name="updateSpecializationDto">The DTO containing updated specialization information.</param>
    /// <returns>A task representing the asynchronous operation, containing the updated specialization as a DTO.</returns>
    public async Task<SpecializationDto> UpdateSpecializationData(SpecializationByCriteriaDTO updateSpecializationDto, string specializationCode)
    {
        try
        {
            var specialization = await GetSpecializationByCode(specializationCode);

            specialization.UpdateSpecialization(updateSpecializationDto);
            
            return await UpdatePatient(specialization);
            ;
        }
        catch (KeyNotFoundException ex)
        {
            throw new Exception("Patient not found.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    private async Task<SpecializationDto> UpdatePatient(Specialization specialization)
    {
        try
        {
            await UpdateSpecializationInRepository(specialization);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return SpecializationMapper.ToDto(specialization);
    }
    
    private async Task UpdateSpecializationInRepository(Specialization specialization)
    {
        await _specializationRepository.UpdateSpecializationAsync(specialization);
    }

    /// <summary>
    /// Retrieves a specialization by its name.
    /// </summary>
    /// <param name="specializationCode">The name of the specialization to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the specialization as a DTO.</returns>
    private async Task<Specialization> GetSpecializationByCode(string specializationCode)
    {
        var specialization = await _specializationRepository.GetByCodeAsync(specializationCode);

        if (specialization == null)
            throw new KeyNotFoundException($"{specializationCode} specialization not found.");

        return specialization;
    }

    /// <summary>
    /// Deletes a specialization by its name.
    /// </summary>
    /// <param name="name">The name of the specialization to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteSpecialization(string name)
    {
        var specialization = await GetSpecializationByCode(name);
        await _specializationRepository.RemoveSpecializationAsync(specialization);
    }

    public async Task<IEnumerable<SpecializationDto>?> GetSpecializationsByCriteria(SpecializationByCriteriaDTO criteriaDto){
        try
        {
            var specializations = await _specializationRepository.SearchPatientsByFiltersAsync(criteriaDto);

            var specializationDtoList = specializations.Select(SpecializationMapper.ToDto).ToList();

            return specializationDtoList;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
