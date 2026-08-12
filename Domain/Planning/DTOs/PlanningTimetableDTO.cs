public class PlanningTimetableDTO
{
    /// <summary>
    /// The license number of the staff member or doctor.
    /// </summary>
    public string LicenseNumber { get; set; }

    /// <summary>
    /// The date associated with the timetable, in the format yyyyMMdd.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// The schedule for the specified date (e.g., start and end times).
    /// </summary>
    public string TimeShiftEntrance { get; set; }
    public string TimeShiftExit { get; set; }

    public PlanningTimetableDTO(string licenseNumber, DateTime date, string timeShiftEntrance, string timeShiftExit)
    {
        LicenseNumber = licenseNumber;
        Date = int.Parse(date.ToString("yyyyMMdd")).ToString();
        TimeShiftEntrance = timeShiftEntrance;
        TimeShiftExit = timeShiftExit;
    }

    public PlanningTimetableDTO()
    {
    }
}