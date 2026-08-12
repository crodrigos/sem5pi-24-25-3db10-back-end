namespace App.Onion.Application.Dtos;

public class EmergencyContactDto()
{
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhoneNumber { get; set; }

    public EmergencyContactDto(string? emergencyContactName, string? emergencyContactPhoneNumber) : this()
    {
        EmergencyContactName = emergencyContactName;
        EmergencyContactPhoneNumber = emergencyContactPhoneNumber;
    }
}