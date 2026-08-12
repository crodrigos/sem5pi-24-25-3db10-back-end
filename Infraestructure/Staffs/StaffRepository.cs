using dddnet8.Domain.Staffs.DTO;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnet8.Infraestructure.Staffs;

public class StaffRepository : BaseRepository<Domain.Staffs.Staff, Guid>, IStaffRepository
{
    private readonly ApplicationDbContext _context;

    public StaffRepository(ApplicationDbContext dbContext) : base(dbContext.Staff)
    {
        _context = dbContext;
    }

    /// <summary>
    ///     Adds a new staff member to the repository asynchronously.
    /// </summary>
    /// <param name="staff">The staff entity to be added.</param>
    public async Task AddStaffAsync(Domain.Staffs.Staff staff)
    {
        try
        {
            await AddAsync(staff);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the staff.", ex);
        }
    }

    /// <summary>
    ///     Retrieves a staff member by their license number asynchronously.
    /// </summary>
    /// <param name="licenseNumber">The license number of the staff member.</param>
    /// <returns>The staff member with the specified license number or null if not found.</returns>
    public async Task<Domain.Staffs.Staff?> GetByLicenseNumberAsync(LicenseNumber licenseNumber)
    {
        try
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.LicenseNumber == licenseNumber)
                   ?? throw new KeyNotFoundException($"Staff with license number {licenseNumber} not found.");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching the staff by license number.", ex);
        }
    }

    /// <summary>
    ///     Searches for staff members based on provided filter criteria asynchronously.
    /// </summary>
    /// <param name="criteria">The filter criteria for the search.</param>
    /// <returns>A list of staff members that match the criteria.</returns>
    public async Task<IEnumerable<Domain.Staffs.Staff>> SearchStaffByFiltersAsync(StaffCriteria criteria)
    {
        try
        {
            IQueryable<Domain.Staffs.Staff> query = _context.Staff;

            query = ApplyFirstNameFilter(query, criteria.FirstName);
            query = ApplyLastNameFilter(query, criteria.LastName);
            query = ApplyFullNameFilter(query, criteria.FullName);
            query = ApplyEmailFilter(query, criteria.Email);
            query = ApplyPhoneNumberFilter(query, criteria.PhoneNumber);
            query = ApplyLicenseNumberFilter(query, criteria.LicenseNumber);
            query = ApplySpecializationNameFilter(query, criteria.SpecializationName);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while searching for staff by filters.", ex);
        }
    }

    /// <summary>
    ///     Updates the data of a staff member asynchronously.
    /// </summary>
    /// <param name="staff">The staff entity with updated data.</param>
    public async Task UpdateStaffDataAsync(Domain.Staffs.Staff staff)
    {
        try
        {
            _context.Staff.Update(staff);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception("A concurrency error occurred while updating the staff.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the staff.", ex);
        }
    }

    /// <summary>
    ///     Retrieves a staff member's license number by their email address asynchronously.
    ///     Not yet implemented.
    /// </summary>
    /// <param name="userEmail">The email address of the staff member.</param>
    /// <returns>The license number associated with the email.</returns>
    public Task<LicenseNumber> GetLicenseNumberByEmailAddressAsync(EmailAddress userEmail)
    {
        throw new NotImplementedException("This method is not yet implemented.");
    }

    /// <summary>
    ///     Retrieves a list of staff members marked for deletion asynchronously.
    /// </summary>
    /// <returns>A list of staff members flagged for deletion.</returns>
    public async Task<List<Domain.Staffs.Staff>> GetStaffMarkedForDeletionAsync()
    {
        try
        {
            var staffsList = _context.Staff.ToList();
            return staffsList.Where(p => p.DeletionStatus.IsToDelete).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching staff marked for deletion.", ex);
        }
    }

    /// <summary>
    ///     Removes a staff member from the repository asynchronously.
    /// </summary>
    /// <param name="staff">The staff entity to be removed.</param>
    public async Task RemoveStaffAsync(Domain.Staffs.Staff staff)
    {
        try
        {
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while removing the staff. Please try again.", ex);
        }
    }

    #region Private Methods for Filtering

    /// <summary>
    ///     Applies a name filter to the query.
    /// </summary>
    /// <param name="query">The query to be filtered.</param>
    /// <param name="name">The name to filter by.</param>
    /// <returns>The filtered query.</returns>
    private IQueryable<Domain.Staffs.Staff> ApplyFullNameFilter(IQueryable<Domain.Staffs.Staff> query, string name)
    {
        
        if (!string.IsNullOrEmpty(name))
        {
            return query.Where(p =>EF.Functions.Like((string)p.FullName, $"%{name}%")); 
        }

        return query;
    }
    
    
    private IQueryable<Domain.Staffs.Staff> ApplyFirstNameFilter(IQueryable<Domain.Staffs.Staff> query, string? firstName)
    {
        if (!string.IsNullOrEmpty(firstName))
        {
            query = query.Where(p => EF.Functions.Like((string)p.FirstName, $"%{firstName}%"));
        }
        return query; 
    }
    
    private IQueryable<Domain.Staffs.Staff> ApplyLastNameFilter(IQueryable<Domain.Staffs.Staff> query, string? lastName)
    {
        if (!string.IsNullOrEmpty(lastName))
        {
            query = query.Where(p => EF.Functions.Like((string)p.LastName, $"%{lastName}%"));
        }
        return query;
    }

    /// <summary>
    ///     Applies an email filter to the query.
    /// </summary>
    /// <param name="query">The query to be filtered.</param>
    /// <param name="email">The email to filter by.</param>
    /// <returns>The filtered query.</returns>
    private static IQueryable<Domain.Staffs.Staff> ApplyEmailFilter(IQueryable<Domain.Staffs.Staff> query, string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            return query.Where(p => p.ContactInfo.EmailAddress.Equals(EmailAddress.Create(email)));
        }

        return query;
    }

    /// <summary>
    ///     Applies a phone number filter to the query.
    /// </summary>
    /// <param name="query">The query to be filtered.</param>
    /// <param name="phoneNumber">The phone number to filter by.</param>
    /// <returns>The filtered query.</returns>
    private static IQueryable<Domain.Staffs.Staff> ApplyPhoneNumberFilter(IQueryable<Domain.Staffs.Staff> query, string phoneNumber)
    {
        if (!string.IsNullOrEmpty(phoneNumber))
        {
            return query.Where(p => EF.Functions.Like(p.ContactInfo.PhoneNumber.Number, $"%{phoneNumber}%"));
        }

        return query;
    }

    /// <summary>
    ///     Applies a license number filter to the query.
    /// </summary>
    /// <param name="query">The query to be filtered.</param>
    /// <param name="licenseNumber">The license number to filter by.</param>
    /// <returns>The filtered query.</returns>
    private static IQueryable<Domain.Staffs.Staff> ApplyLicenseNumberFilter(IQueryable<Domain.Staffs.Staff> query,
        string licenseNumber)
    {
        if (!string.IsNullOrEmpty(licenseNumber))
        {
            return query.Where(p => p.LicenseNumber.Equals(new LicenseNumber(licenseNumber)));
        }
        return query;
    }

    /// <summary>
    ///     Applies a specialization filter to the query.
    /// </summary>
    /// <param name="query">The query to be filtered.</param>
    /// <param name="specializationName">The specialization to filter by.</param>
    /// <returns>The filtered query.</returns>
    private static IQueryable<Domain.Staffs.Staff> ApplySpecializationNameFilter(IQueryable<Domain.Staffs.Staff> query,
        string specializationName)
    {
        if (!string.IsNullOrEmpty(specializationName))
        {
            return query.Where(p => p.Specialization.Name.Equals(specializationName));
        }

        return query;
    }

    #endregion
}