using App.Domain.SystemUser;

namespace dddnet8.Domain.SystemUsers.DTO;

public abstract class SystemUserMapper
{
    public static  SystemUserDto ToDto(SystemUser systemUser)
    {
        return new SystemUserDto(
            systemUser.Username.ToString(), 
            systemUser.EmailAddress.ToString(),
            systemUser.Role.ToString());
    }
}