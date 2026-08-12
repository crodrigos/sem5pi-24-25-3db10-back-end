using dddnet8.Domain.Shared;
using SurgicalManagement.Domain.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dddnet8.Domain.SystemUsers
{
    /// <summary>
    /// Interface for repository operations related to system users.
    /// </summary>
    public interface ISystemUserRepository : IRepository<SystemUser, Guid>
    {
        /// <summary>
        /// Retrieves a user by their username (email address).
        /// </summary>
        /// <param name="username">The email address of the user.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found.</returns>
        Task<SystemUser> GetUserByUsernameAsync(EmailAddress username);

        /// <summary>
        /// Retrieves a collection of users by their role.
        /// </summary>
        /// <param name="role">The role of the users to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing a collection of users.</returns>
        Task<IEnumerable<SystemUser>> GetUsersByRoleAsync(UserRole role);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A task that represents the asynchronous operation, containing the user if found.</returns>
        Task<SystemUser> GetUserByEmailAsync(EmailAddress email);

        /// <summary>
        /// Activates a user account.
        /// </summary>
        /// <param name="user">The user account to activate.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ActivateUserAccountAsync(SystemUser user);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUserAsync(SystemUser user);

        /// <summary>
        /// Removes a user from the repository.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveUserAsync(SystemUser user);

        /// <summary>
        /// Updates the data of an existing user in the repository.
        /// </summary>
        /// <param name="user">The user with updated data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateUserDataAsync(SystemUser user);

        Task<List<SystemUser>> GetUsersMarkedForDeletionAsync();
    }
}
