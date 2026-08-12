using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations.DTO;

namespace dddnet8.Domain.Specializations.Interfaces
{
    /// <summary>
    /// Interface para o repositório de especializações.
    /// Define os contratos de operações para adicionar, buscar, atualizar e remover especializações,
    /// além de permitir buscas com critérios de filtro.
    /// </summary>
    public interface ISpecializationRepository  : IRepository<Specialization, Guid>
    {
        /// <summary>
        /// Adiciona uma nova especialização ao repositório de forma assíncrona.
        /// </summary>
        /// <param name="specialization">A especialização a ser adicionada.</param>
        /// <returns>Tarefa representando a operação assíncrona.</returns>
        Task AddSpecializationAsync(Specialization specialization);

        /// <summary>
        /// Recupera uma especialização pelo nome de forma assíncrona.
        /// </summary>
        /// <param name="name">Nome da especialização a ser buscada.</param>
        /// <returns>A especialização correspondente ou null se não encontrada.</returns>
        Task<Specialization?> GetByCodeAsync(string specializationCode);
        
        Task<Specialization?> GetByNameAsync(string name);

        /// <summary>
        /// Atualiza os dados de uma especialização existente de forma assíncrona.
        /// </summary>
        /// <param name="specialization">A especialização com dados atualizados.</param>
        /// <returns>Tarefa representando a operação assíncrona.</returns>
        Task UpdateSpecializationAsync(Specialization specialization);

        /// <summary>
        /// Remove uma especialização do repositório de forma assíncrona.
        /// </summary>
        /// <param name="specialization">A especialização a ser removida.</param>
        /// <returns>Tarefa representando a operação assíncrona.</returns>
        Task RemoveSpecializationAsync(Specialization specialization);

        Task<IEnumerable<Specialization>?> SearchPatientsByFiltersAsync(SpecializationByCriteriaDTO criteriaDto);
    }
}
