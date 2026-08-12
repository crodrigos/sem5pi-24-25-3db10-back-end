using dddnet8.Domain.Specializations.DTO;

namespace dddnet8.Domain.Specializations.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that manages specializations.
    /// </summary>
    public interface ISpecializationService
    {
        /// <summary>
        /// Creates a new specialization.
        /// </summary>
        /// <param name="createSpecializationDto">The DTO containing specialization information.</param>
        /// <returns>A task representing the asynchronous operation, containing the created specialization as a DTO.</returns>
        Task<SpecializationDto> CreateSpecialization(SpecializationDto createSpecializationDto);

        /// <summary>
        /// Updates an existing specialization.
        /// </summary>
        /// <param name="specializationCode">The name of the specialization to update.</param>
        /// <param name="updateSpecializationDto">The DTO containing updated specialization information.</param>
        /// <returns>A task representing the asynchronous operation, containing the updated specialization as a DTO.</returns>
        Task<SpecializationDto> UpdateSpecializationData(SpecializationByCriteriaDTO updateSpecializationDto, string specializationCode);

        /// <summary>
        /// Deletes a specialization by its name.
        /// </summary>
        /// <param name="name">The name of the specialization to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteSpecialization(string name);

        Task<IEnumerable<SpecializationDto>?> GetSpecializationsByCriteria(SpecializationByCriteriaDTO specializationByCriteriaDto);
    }
}
