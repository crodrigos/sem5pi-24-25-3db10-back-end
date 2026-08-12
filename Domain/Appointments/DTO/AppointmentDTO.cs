namespace dddnet8.Domain.Appointments.DTO
{
    public class CreateAppointmentDTO
    {
        /// <summary>
        /// A data do agendamento.
        /// </summary>
        public DateTime AppointmentDate { get; set; }
        
        /// <summary>
        /// A lista de números de licença dos membros da equipe envolvidos no agendamento.
        /// </summary>
        public List<string> TeamLicenseNumbers { get; set; }
        
        /// <summary>
        /// A hora de início do agendamento.
        /// </summary>
        public string SurgeryStartTime { get; set; }
        
        /// <summary>
        /// O nome ou identificador da sala de cirurgia onde o agendamento ocorrerá.
        /// </summary>
        public string SurgeryRoom { get; set; }
        
        public string OperationRequestCode { get; set; }
        
        public string OperationTypeCode { get; set; }

        /// <summary>
        /// Construtor para inicializar o DTO com valores padrão.
        /// </summary>
        public CreateAppointmentDTO()
        {
            TeamLicenseNumbers = new List<string>();
        }

        /// <summary>
        /// Construtor com parâmetros para inicializar o DTO diretamente.
        /// </summary>
        public CreateAppointmentDTO(string appointmentDate, List<string> teamLicenseNumbers, string startTime, string surgeryRoom,
            string operationRequestCode, string operationTypeCode)
        {
            AppointmentDate = DateTime.Parse(appointmentDate);
            TeamLicenseNumbers = teamLicenseNumbers ?? new List<string>();
            SurgeryStartTime = startTime;
            SurgeryRoom = surgeryRoom;
            OperationRequestCode = operationRequestCode;
            OperationTypeCode = operationTypeCode;
        }
    }
    
    public class TimetableRequest
    {
        public string StartTime { get; set; }
        public string LicenseNumber { get; set; }
    }
}
