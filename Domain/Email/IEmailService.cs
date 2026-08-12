using App.Domain.SystemUser;
using dddnet8.Domain.SystemUsers;

namespace dddnet8.Infraestructure.Email;

public interface IEmailService
{
    Task ActivationAccount(string to, string token);
    
    Task SendAdminWarningNotification(IEnumerable<SystemUser> to, string username);
    Task ResetPasswordNotification(string userDtoEmailAddress, string token);
    Task SendAccountActivationConfirmation(SystemUserDto user);
    Task NotifyClientAboutUpdate(EmailAddress contactInformationString);
    Task NotifyAdminsAboutDelete(EmailAddress adminsEmailAddress, List<string> deletedList, string whatDeleted);
    Task RequestDpoToDeleteMyAccount(string patientEmail, string patientMedicalRecordNumber);
}