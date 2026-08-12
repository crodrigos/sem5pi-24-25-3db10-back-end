using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using SurgicalManagement.Domain.Domain;

namespace dddnet8.Domain.Staffs.Services;

public class StaffCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public StaffCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken);
            await CleanupStaffAsync(stoppingToken);
        }
    }

    private async Task CleanupStaffAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var staffRepository = scope.ServiceProvider.GetRequiredService<IStaffRepository>();
        var staffLogService = scope.ServiceProvider.GetRequiredService<ILogService<Staff>>();

        var staffList = await staffRepository.GetStaffMarkedForDeletionAsync();
        var staffEmailsToDelete = new List<string>();

        foreach (var staff in staffList)
        {
            if (staff.CanDelete())
            {
                staffEmailsToDelete.Add(staff.ContactInfo.EmailAddress.ToString());
                await LogStaffDeletion(staffLogService, staff);
                await staffRepository.RemoveStaffAsync(staff);
            }
        }

        if (staffEmailsToDelete.Count > 0)
        {
            await NotifyAdminsAboutStaffDeletion(staffEmailsToDelete);
        }
    }

    private async Task NotifyAdminsAboutStaffDeletion(List<string> staffEmailsToDelete)
    {
        using var scope = _serviceProvider.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var systemUserRepository = scope.ServiceProvider.GetRequiredService<ISystemUserRepository>();

        var admins = await systemUserRepository.GetUsersByRoleAsync(UserRole.Admin);
        var adminsEmails = admins.Select(admin => admin.EmailAddress).ToList();

        foreach (var adminEmail in adminsEmails)
        {
            await emailService.NotifyAdminsAboutDelete(adminEmail, staffEmailsToDelete, "Staff");
        }
    }

    private async Task LogStaffDeletion(ILogService<Staff> staffLogService, Staff staff)
    {
        await staffLogService.LogActionAsync("delete", staff);
    }
}