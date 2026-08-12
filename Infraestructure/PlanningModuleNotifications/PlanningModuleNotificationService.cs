using System.Text;
using dddnet8.Domain.PlanningModuleNotifications;

namespace dddnet8.Infraestructure.PlanningModuleNotifications;

public class PlanningModuleNotificationService : IPlanningModuleNotificationService
{
    private readonly HttpClient _httpClient;

    public PlanningModuleNotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task NotifyAsync(string message)
    {
        var content = new StringContent(message, Encoding.UTF8, "application/json");

        // For now I dont have a separated controller because planning module is not defined
        var response = await _httpClient.PostAsync("/planning/notify", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to notify Planning Module: {response.ReasonPhrase}");
        }
    }
}