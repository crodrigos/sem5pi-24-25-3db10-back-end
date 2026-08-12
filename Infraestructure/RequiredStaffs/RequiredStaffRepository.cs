using System;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.Specializations;

namespace dddnet8.Infraestructure.RequiredStaffs;

public class RequiredStaffRepository : BaseRepository<RequiredStaff, Guid>, IRequiredStaffRepository
{

    private ApplicationDbContext _context;

    /// <summary>
    /// Represents a repository for managing <see cref="RequiredStaff"/> entities.
    /// </summary>
    /// <remarks>
    /// This    /// /// ository provides methods for interacting with the <see cref="RequiredStaff"/> data in the database.
    /// </remarks>
    /// <param name="context">The database context used to access the <see cref="RequiredStaff"/> entities.</param>
    public RequiredStaffRepository(ApplicationDbContext context) : base(context.RequiredStaff)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of <see cref="RequiredStaff"/> entities that match the specified <see cref="OperationType"/>.
    /// </summary>
    /// <param name="operationType">The operation type to filter the <see cref="RequiredStaff"/> entities by.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="RequiredStaff"/> entities that match the specified operation type.</returns>
    public async Task<List<RequiredStaff>> GetByOperationTypeAsync(OperationType operationType)
    {
        List<RequiredStaff> all = await _context.RequiredStaff.Include(rs => rs.operationType).ToListAsync();   
        List<RequiredStaff> filtered = all.Where<RequiredStaff>(x => x.operationType.Id==operationType.Id).ToList<RequiredStaff>();
        return filtered;
    }

    /// <summary>
    /// Removes all <see cref="RequiredStaff"/> entities that match the specified <see cref="OperationType"/>.
    /// </summary>
    /// <param name="operationType">The operation type to filter the <see cref="RequiredStaff"/> entities by.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the operation was successful.</returns>
    public async Task<bool> RemoveByOperationType(OperationType operationType)
    {
        List<RequiredStaff> all = await this.GetAllAsync();

        List<RequiredStaff> toRemove = all.Where<RequiredStaff>(x => x.operationType.Id == operationType.Id)
            .ToList<RequiredStaff>();

        toRemove.ForEach(x => { this.Remove(x); });

        this.Save();
        
        return toRemove.Count > 0;
    }

    public async Task<bool> Save()
    {
        int n = _context.SaveChanges();
        return n > 0;
    }

    public async Task<List<OperationTypeCode>> GetOperationTypesBySpecialization(Specialization specialization)
    {

        var requiredStaff = await _context.RequiredStaff
            .Where(rs => rs.specialization == specialization)
            .Include(rs => rs.operationType)
            .ToListAsync();


        var operationTypeCodes = requiredStaff
            .Select(rs => rs.operationType.OperationTypeCode)
            .Distinct()
            .ToList();

        return operationTypeCodes;
    }
}

