using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Shared;
using Microsoft.EntityFrameworkCore;
using SurgicalManagement.Domain.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dddnet8.Infraestructure.SystemUsers
{
    public class SystemUserRepository : BaseRepository<SystemUser, Guid>, ISystemUserRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemUserRepository(ApplicationDbContext dbContext) : base(dbContext.SystemUser)
        {
            _context = dbContext;
        }

        public async Task<SystemUser> GetUserByUsernameAsync(EmailAddress username)
        {
            return await _context.SystemUser
                .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public async Task<IEnumerable<SystemUser>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.SystemUser
                .Where(u => u.Role.Equals(role))
                .ToListAsync();
        }

        public async Task<SystemUser> GetUserByEmailAsync(EmailAddress email)
        {
            return await _context.SystemUser
                .FirstOrDefaultAsync(u => u.EmailAddress.Equals(email));
        }

        public async Task ActivateUserAccountAsync(SystemUser user)
        {
            _context.SystemUser.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(SystemUser user)
        {
            await _context.SystemUser.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(SystemUser user)
        {
            _context.SystemUser.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserDataAsync(SystemUser user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingUser = await GetUserByUsernameAsync(user.Username);
                if (existingUser == null) throw new KeyNotFoundException("Patient not found.");

                // Optionally, copy data from existingUser to user if necessary
                _context.SystemUser.Remove(existingUser);
                await _context.SystemUser.AddAsync(user);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Rethrow the caught exception
            }
        }
        
        public async Task<List<SystemUser>> GetUsersMarkedForDeletionAsync()
        {
            var usersList = _context.SystemUser.ToList();
            
            return usersList.Where(p => p.DeletionStatus.IsToDelete).ToList();
        }
    }
}
