using System;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.OperationTypes;

public class OperationTypeRepository : BaseRepository<OperationType, Guid>, IOperationTypeRepository
{

    private readonly ApplicationDbContext _context;

    public OperationTypeRepository(ApplicationDbContext dbContext) : base(dbContext.OperationTypes)
    {
        _context = dbContext;
    }
    
    public bool CanConnectToDatabase() => _context.Database.CanConnect();

    public Task<List<OperationType>> GetAllOperationTypesAsync()
    {
        return this._context.OperationTypes.ToListAsync();
    }

    public async Task<List<OperationType>> GetByStatusAsync(Status status)
    {
        List<OperationType> all = await this.GetAllAsync();
        
        if (all == null)
        {
            throw new KeyNotFoundException("No operation types found.");
        }

        return all.Where<OperationType>(x => x.Status == status).ToList<OperationType>();
    }

    public async Task<OperationType> UpdateAsync(OperationType operationType)
    {
        try
        {
            _context.OperationTypes.Update(operationType);;
            await _context.SaveChangesAsync();
            return operationType;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the operation type.", ex);
        }
    }

    public async Task<OperationType> AddOperationType(OperationType operationType)
    {
        try
        {
            OperationType op = await base.AddAsync(operationType);
            await _context.SaveChangesAsync();
            return op;
        }
        catch (Exception e)
        {
            throw new Exception("Error inserting in database");
        }
    }
    
    public async Task<OperationTypeCode?> GetLastOperationTypeCode()
    {
        
        var lastCode = _context.OperationTypes
            .OrderByDescending(o => o.OperationTypeCode)  
            .Select(o => o.OperationTypeCode)  
            .FirstOrDefault();  

        return lastCode;  
    }

    
    public int Size()
    {
        return _context.Set<OperationType>().Count();
    }

    public Task<OperationType?> GetByOperationTypeCode(OperationTypeCode dtoOperationTypeId)
    {
        return _context.OperationTypes.Where(o => o.OperationTypeCode == dtoOperationTypeId).FirstOrDefaultAsync();
    }
}
