using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Specializations;
using dddnet8.Domain.Specializations.DTO;
using dddnet8.Domain.Specializations.Interfaces;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.Specializations
{
    public class SpecializationRepository : BaseRepository<Specialization, Guid>, ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecializationRepository(ApplicationDbContext dbContext) : base(dbContext.Specializations)
        {
            _context = dbContext;
        }

        /// <summary>
        /// Adds a new specialization to the repository asynchronously.
        /// </summary>
        /// <param name="specialization">The specialization entity to be added.</param>
        public async Task AddSpecializationAsync(Specialization specialization)
        {
            try
            {
                await AddAsync(specialization);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the specialization.", ex);
            }
        }

        /// <summary>
        /// Retrieves a specialization by its name asynchronously.
        /// </summary>
        /// <param name="specializationCode">The name of the specialization.</param>
        /// <returns>The specialization with the specified name or null if not found.</returns>
        public async Task<Specialization?> GetByCodeAsync(string specializationCode)
        {
            try
            {
                return await _context.Specializations.FirstOrDefaultAsync(s => s.Code == SpecializationCode.Create(specializationCode));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the specialization.", ex);
            }
        }

        /// <summary>
        /// Retrieves a specialization by its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the specialization.</param>
        /// <returns>The specialization with the specified name or null if not found.</returns>
        public async Task<Specialization?> GetByNameAsync(string name)
        {
            var Name = Domain.Patients.V.O.Name.Create(name);
            try
            {
                return await _context.Specializations.FirstOrDefaultAsync(s => s.Name == Name);
                       
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the specialization by name.", ex);
            }
        }

        /// <summary>
        /// Updates the data of a specialization asynchronously.
        /// </summary>
        /// <param name="specialization">The specialization entity with updated data.</param>
        public async Task UpdateSpecializationAsync(Specialization specialization)
        {
            try
            {
                _context.Specializations.Update(specialization);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred while updating the specialization.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the specialization.", ex);
            }
        }

        /// <summary>
        /// Removes a specialization from the repository asynchronously.
        /// </summary>
        /// <param name="specialization">The specialization entity to be removed.</param>
        public async Task RemoveSpecializationAsync(Specialization specialization)
        {
            try
            {
                _context.Specializations.Remove(specialization);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while removing the specialization.", ex);
            }
        }

        public async Task<IEnumerable<Specialization>?> SearchPatientsByFiltersAsync(SpecializationByCriteriaDTO criteriaDto)
        {
            try
            {
                IQueryable<Specialization> query = _context.Specializations;

                query = ApplySpecializationCodeFilter(query, criteriaDto.SpecializationCode);
                query = ApplySpecializationaNameFilter(query, criteriaDto.Name);
                query = ApplySpecializationDescriptionFilter(query, criteriaDto.Description);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for staff by filters.", ex);
            }
        }

        
        // TODOO ---------------> ESTA MERDA DA DESCRIPTION DO JOAO PODE FAZER ISSO EXPLODIR.
        private IQueryable<Specialization> ApplySpecializationDescriptionFilter(IQueryable<Specialization> query, string? criteriaDescription)
        {
            
            Console.WriteLine(criteriaDescription);
            
            if (!string.IsNullOrEmpty(criteriaDescription))
            {
                query = query.Where(p => EF.Functions.Like((string)p.Description, $"%{criteriaDescription}%"));

            }
            return query;
        }

        private IQueryable<Specialization> ApplySpecializationaNameFilter(IQueryable<Specialization> query, string? criteriaName)
        {
            if (!string.IsNullOrEmpty(criteriaName))
            {
                query = query.Where(p => EF.Functions.Like((string)p.Name, $"%{criteriaName}%"));
            }
            return query;
        }

        private IQueryable<Specialization> ApplySpecializationCodeFilter(IQueryable<Specialization> query, string? code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(p => EF.Functions.Like((string)p.Code, $"%{code}%"));
            }
            return query; 
        }
    }
}
