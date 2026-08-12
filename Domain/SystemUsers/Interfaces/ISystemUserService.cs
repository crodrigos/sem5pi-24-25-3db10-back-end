using System.Security.Claims;
using App.Domain.SystemUser;
using App.SystemUserStuff;
using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;

public interface ISystemUserService
{
    Task<SystemUserDto> CreateUser(SystemUserRequestDto systemUserRequestDto);
    Task<SystemUser> GetUserByUsername(string username);
    Task<IEnumerable<SystemUser>> GetUsersByRole(UserRole role);
    Task ActivateUserAccount(string userEmailAddress, string requestPassword);
    Task<SystemUser> GetUserByEmail(string userEmail);
    Task ResetUserPassword(SystemUser user, string password);
    Task<(SystemUserDto userDto, string token, string errorMessage)> CreateUserFromIAM(ClaimsPrincipal claimsPrincipal);
    Task<(bool IsSuccess, string Message)> DeleteUser(string username);
    
    Task MarkUserForDeletion(string toString);
}