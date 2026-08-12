using System.Runtime.InteropServices.JavaScript;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.Staffs.DTO;

public class TimeSlotDTO
{
    public DateOnly DateOnly { get; set; }
    public LicenseNumber LicenseNumber { get; set; }
    public TimeShift TimeShift { get; set; }


    public TimeSlotDTO(DateOnly dateOnly, string licenseNumber, int startTimeSlot, int endTimeSlot)
    {
        DateOnly = dateOnly;
        LicenseNumber = new LicenseNumber(licenseNumber);
        TimeShift = new TimeShift(TimeSpan.FromMinutes(startTimeSlot), TimeSpan.FromMinutes(endTimeSlot));

    }
    
}