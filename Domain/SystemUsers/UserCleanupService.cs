using App.Onion.Domain.Interfaces.PatientRepository;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using Microsoft.Extensions.DependencyInjection;
using SurgicalManagement.Domain.Domain;

public class UserCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public UserCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromDays(2), stoppingToken);
            await CleanupUsersAsync(stoppingToken);
        }
    }

    private async Task CleanupUsersAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<ISystemUserRepository>();
        var userLogService = scope.ServiceProvider.GetRequiredService<ILogService<SystemUser>>();

        var users = await userRepository.GetUsersMarkedForDeletionAsync();
        var listUsersToDelete = new List<string>();

        foreach (var user in users)
        {
            if (user.CanDelete())
            {
                listUsersToDelete.Add(user.EmailAddress.ToString());
                await LogUserDeletion(userLogService, user);
                await userRepository.RemoveUserAsync(user);
            }
        }

        if (listUsersToDelete.Count > 0)
        {
            await NotifyAdminsAboutPatientsDeletion(listUsersToDelete);
        }
    }

        

    private async Task NotifyAdminsAboutPatientsDeletion(List<string> listUsersToDelete)
    {
        using var scope = _serviceProvider.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var systemUserRepository = scope.ServiceProvider.GetRequiredService<ISystemUserRepository>();

        var admins = await systemUserRepository.GetUsersByRoleAsync(UserRole.Admin);
        var adminsEmailAddress = admins.Select(admin => admin.EmailAddress).ToList();

       
        foreach (var adminEmail in adminsEmailAddress)
        {
            await emailService.NotifyAdminsAboutDelete(adminEmail, listUsersToDelete, "Patients");
        }
    }


    private async Task LogUserDeletion(ILogService<SystemUser> patientLogService, SystemUser user)
    {
        await patientLogService.LogActionAsync("delete", user);
    }
}
