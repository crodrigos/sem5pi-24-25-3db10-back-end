public class PlanningAgendaStaffDTO
{
    /// <summary>
    /// The license number of the staff member or doctor.
    /// </summary>
    public string LicenseNumber { get; set; }

    /// <summary>
    /// The date associated with the agenda, in the format yyyyMMdd.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// The list of occupied time slots (e.g., ["08:00-09:00", "10:00-11:30"]).
    /// </summary>
    public List<string> Schedule { get; set; }

    public PlanningAgendaStaffDTO(string licenseNumber, DateOnly date, List<string> schedule)
    {
        LicenseNumber = licenseNumber;
        Date = int.Parse(date.ToString("yyyyMMdd")).ToString();
        Schedule = schedule; // List of occupied time intervals.
    }

    public PlanningAgendaStaffDTO()
    {
        Schedule = new List<string>();
    }
}