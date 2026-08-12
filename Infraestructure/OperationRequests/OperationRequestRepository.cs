using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dddnet8.Infraestructure.OperationRequests;

public class OperationRequestRepository : IOperationRequestRepository
{
    private readonly ApplicationDbContext _context;

    public OperationRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OperationRequest>> GetAllAsync()
    {
        return await _context.OperationRequests.ToListAsync();
    }

    public async Task<OperationRequest> GetByIdAsync(Guid id)
    {
        return await _context.OperationRequests.FindAsync(id);
    }

    public async Task<List<OperationRequest>> GetByIdsAsync(List<Guid> ids)
    {
        return await _context.OperationRequests
            .Where(or => ids.Contains(or.Id))
            .ToListAsync();
    }

    public async Task<OperationRequest> AddAsync(OperationRequest obj)
    {
        await _context.OperationRequests.AddAsync(obj);
        await _context.SaveChangesAsync(); // Commit the transaction
        return obj; // Return the added entity
    }

    public void Remove(OperationRequest obj)
    {
        _context.OperationRequests.Remove(obj);
    }

    // -----------------------------------------------------------------------------------------------------------------
    public async Task AddOperationRequestAsync(OperationRequest operationRequest)
    {
        try
        {
            await _context.OperationRequests.AddAsync(operationRequest);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("OperationRequestRepository.AddOperationRequestAsync failed", e);
        }
    }


    public async Task<IEnumerable<OperationRequest>> SearchOperationRequestsByFiltersAsync(
        OperationRequestCriteria criteria)
    {
        IQueryable<OperationRequest> query = _context.OperationRequests;

        query = ApplyDoctorFilter(query, criteria.DoctorId);
        query = ApplyPatientFilter(query, criteria.PatientId);
        query = ApplyOperationTypeFilter(query, criteria.OperationTypeId);
        query = ApplyDeadlineFilter(query, criteria.Deadline);
        query = ApplyPriorityFilter(query, criteria.Priority);
        query = ApplyStatusFilter(query, criteria.Status);

        return await query.ToListAsync();
    }

    public async Task RemoveOperationRequest(OperationRequest operationRequest)
    {
        try
        {
            _context.OperationRequests.Remove(operationRequest);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("OperationRequestRepository.RemoveOperationRequestAsync failed", e);
        }
    } 
    
    public async Task<bool> ExistsRecentRequest(MedicalRecordNumber patientId, LicenseNumber doctorId, DateTime since)
    {
        return await _context.OperationRequests.AnyAsync(or => or.PatientId == patientId && or.DoctorId == doctorId && or.CreatedDate >= since);
    }

    public async Task<int> GetOperationRequestCount()
    {
        return await _context.OperationRequests.CountAsync();   
    }

    public async  Task<OperationRequest?> GetByOperationRequestCode(string operationRequestCode)
    {

        if (operationRequestCode == null)
        {
            throw new ArgumentException("OperationRequestCode cannot be null");
        }
        var opCode = OperationRequestCode.Create(operationRequestCode);
        
        
        return await _context.OperationRequests.FirstOrDefaultAsync(or => or.OperationRequestCode == opCode);
    }

    private IQueryable<OperationRequest> ApplyDoctorFilter(IQueryable<OperationRequest> query, string? doctorId)
    {
        if (!doctorId.IsNullOrEmpty())
        {
            var licenseNumber = new LicenseNumber(doctorId);
            query = query.Where(o =>  o.DoctorId == licenseNumber );
        }

        return query;
    }

    private IQueryable<OperationRequest> ApplyPatientFilter(IQueryable<OperationRequest> query, string? patientId)
    {
        if (!patientId.IsNullOrEmpty())
        {
            var mrn = MedicalRecordNumber.Create(patientId);
            query = query.Where(o => o.PatientId == mrn);
        }

        return query;
    }

    private IQueryable<OperationRequest> ApplyOperationTypeFilter(IQueryable<OperationRequest> query,
        string? operationTypeId)
    {
        if (!operationTypeId.IsNullOrEmpty())
        {
            var opCode = OperationTypeCode.Create(operationTypeId);
            query = query.Where(o => o.OperationTypeId == opCode);
        }

        return query;
    }

    private IQueryable<OperationRequest> ApplyDeadlineFilter(IQueryable<OperationRequest> query, DateTime? deadline)
    {
        if (deadline.HasValue)
        {
            
            query = query.Where(o => o.DeadlineDate.Date == deadline.Value.Date);
        }

        return query;
    }

    private IQueryable<OperationRequest> ApplyPriorityFilter(IQueryable<OperationRequest> query, string? priority)
    {
        if (Enum.TryParse<OperationRequestPriority>(priority, true, out var priorityValue))
        {
            return query.Where(o => o.Priority.Equals(priorityValue));
        }

        return query;
    }
    
    private IQueryable<OperationRequest> ApplyStatusFilter(IQueryable<OperationRequest> query, string? status){
        if (Enum.TryParse<OperationRequestStatus>(status, true, out var statusValue))
        {
            return query.Where(o => o.Status.Equals(statusValue));
        }

        return query;
    }
}