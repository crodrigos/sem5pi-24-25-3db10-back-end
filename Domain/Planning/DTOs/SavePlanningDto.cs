namespace dddnet8.Domain.PlanningModuleNotifications.DTOs
{
    public class SavePlanningDto
    {
        public List<DoctorScheduleDto> doctors { get; set; }
        public List<SurgeryDto> surgeries { get; set; }
        public string room { get; set; }
        public string date { get; set; }
    }

    public class DoctorScheduleDto
    {
        public string doctor_id { get; set; }
        public List<ScheduleDto> schedule { get; set; }
    }

    public class SurgeryDto
    {
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string surgery_id { get; set; }
    }

    public class ScheduleDto
    {
        public string operation { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }
}