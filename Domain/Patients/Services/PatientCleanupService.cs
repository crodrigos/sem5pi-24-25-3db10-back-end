using App.Onion.Domain.Interfaces.PatientRepository;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using Microsoft.Extensions.DependencyInjection;
using SurgicalManagement.Domain.Domain;

public class PatientCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PatientCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            await CleanupPatientsAsync(stoppingToken);
        }
    }

    private async Task CleanupPatientsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var patientRepository = scope.ServiceProvider.GetRequiredService<IPatientRepository>();
        var patientLogService = scope.ServiceProvider.GetRequiredService<ILogService<Patient>>();

        var patients = await patientRepository.GetPatientsMarkedForDeletionAsync();
        var listPatientsToDelete = new List<string>();

        foreach (var patient in patients)
        {
            if (patient.CanDelete())
            {
                listPatientsToDelete.Add(patient.ContactInformation.EmailAddress.ToString());
                await LogPatientDeletion(patientLogService, patient);
                await patientRepository.RemovePatientAsync(patient);
            }
        }

        if (listPatientsToDelete.Count > 0)
        {
            await NotificateAdminsAboutPatientsDeletion(listPatientsToDelete);
        }
    }

        

    private async Task NotificateAdminsAboutPatientsDeletion(List<string> listPatientsToDelete)
    {
        using var scope = _serviceProvider.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var systemUserRepository = scope.ServiceProvider.GetRequiredService<ISystemUserRepository>();

        var admins = await systemUserRepository.GetUsersByRoleAsync(UserRole.Admin);
        var adminsEmailAddress = admins.Select(admin => admin.EmailAddress).ToList();

       
        foreach (var adminEmail in adminsEmailAddress)
        {
            await emailService.NotifyAdminsAboutDelete(adminEmail, listPatientsToDelete, "Patients");
        }
    }


    private async Task LogPatientDeletion(ILogService<Patient> patientLogService, Patient patient)
    {
        await patientLogService.LogActionAsync("delete", patient);
    }
}
