namespace dddnet8.Domain.Appointments.DTO;

public class UpdateAppointmentDTO
{
    public List<string> LicenseNumbers { get; set; }
    public string SurgeryRoom { get; set; }
    public string OperationRequestCode { get; set; }
    public DateOnly Date { get; set; }
    
    public string StartTime { get; set; }
    
    public string EndTime { get; set; } 

    public UpdateAppointmentDTO(List<string> licenseNumbers, DateOnly date, string operationRequestCode, string surgeryRoom, string startTime, string endTime) {
        LicenseNumbers = licenseNumbers;
        SurgeryRoom = surgeryRoom;
        OperationRequestCode = operationRequestCode;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }
}