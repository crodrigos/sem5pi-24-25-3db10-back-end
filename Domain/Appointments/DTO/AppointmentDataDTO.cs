namespace dddnet8.Domain.Appointments.DTO;

public class AppointmentDataDTO{
    public List<string> TeamLicenseNumbers { get; set; }

    public string SurgeryRoom { get; set; }
    
    public string OperationRequestCode { get; set; }
    
    
    public AppointmentDataDTO(){
        TeamLicenseNumbers = new List<string>();
    }
    
    public AppointmentDataDTO(List<string> teamLicenseNumbers, string surgeryRoom,
        string operationRequestCode){
        TeamLicenseNumbers = teamLicenseNumbers ?? new List<string>();
        SurgeryRoom = surgeryRoom;
        OperationRequestCode = operationRequestCode;
    }
}

