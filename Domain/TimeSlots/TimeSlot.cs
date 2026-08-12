using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.Staffs;

/// <summary>
///     Represents a time slot for a staff member, including a specific date, associated license number, and time shift.
/// </summary>
public class TimeSlot : Entity<Guid>, IAggregateRoot
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeSlot"/> class.
    /// </summary>
    /// <param name="date">The specific date for the time slot.</param>
    /// <param name="licenseNumber">The license number associated with the time slot.</param>
    /// <param name="timeShift">The time shift details for the time slot.</param>
    public TimeSlot(DateOnly date, LicenseNumber licenseNumber, TimeShift timeShift)
        : base(Guid.NewGuid())
    {
        Date = date;
        LicenseNumber = licenseNumber ?? throw new ArgumentNullException(nameof(licenseNumber), "License number cannot be null.");
        TimeShift = timeShift ?? throw new ArgumentNullException(nameof(timeShift), "Time shift cannot be null.");
    }

    public TimeSlot() : base(Guid.NewGuid()) {}
    
    /// <summary>
    ///     Gets or sets the specific date for the time slot.
    /// </summary>
    public DateOnly Date { get; private set; }

    /// <summary>
    ///     Gets or sets the license number associated with the time slot.
    /// </summary>
    public LicenseNumber LicenseNumber { get; private set; }

    /// <summary>
    ///     Gets or sets the time shift details for the time slot.
    /// </summary>
    public TimeShift TimeShift { get; private set; }

    /// <summary>
    ///     Updates the license number associated with the time slot.
    /// </summary>
    /// <param name="licenseNumber">The new license number.</param>
    public void UpdateLicenseNumber(LicenseNumber licenseNumber)
    {
        LicenseNumber = licenseNumber ?? throw new ArgumentNullException(nameof(licenseNumber), "License number cannot be null.");
    }

    /// <summary>
    ///     Updates the time shift associated with the time slot.
    /// </summary>
    /// <param name="timeShift">The new time shift.</param>
    public void UpdateTimeShift(TimeShift timeShift)
    {
        TimeShift = timeShift ?? throw new ArgumentNullException(nameof(timeShift), "Time shift cannot be null.");
    }

    /// <summary>
    ///     Returns a string that represents the time slot, including the date and time shift details.
    /// </summary>
    /// <returns>A formatted string showing the date and time shift.</returns>
    public override string ToString()
    {
        return $"{Date:yyyy-MM-dd} - {TimeShift}";
    }
}