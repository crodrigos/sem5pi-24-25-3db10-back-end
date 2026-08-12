using dddnet8.Domain.SystemUsers;

namespace dddnet8.Domain.BackOfficeEmail;

public interface IBackOfficeEmailGenerator
{
    EmailAddress GenerateStaffEmail(string staffCode);
}