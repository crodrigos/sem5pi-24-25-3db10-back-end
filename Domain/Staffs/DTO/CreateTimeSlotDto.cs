namespace dddnet8.Domain.Staffs.DTO;

/// <summary>
///     DTO representing a time slot with a start and end time.
/// </summary>
public class TimeSlotDto
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeSlotDto" /> class with specified start and end times.
    /// </summary>
    /// <param name="startTime">The start time of the time slot.</param>
    /// <param name="endTime">The end time of the time slot.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="endTime" /> is earlier than
    ///     <paramref name="startTime" />.
    /// </exception>
    public TimeSlotDto(DateTime startTime, DateTime endTime)
    {
        if (endTime < startTime)
            throw new ArgumentException("End time must be greater than or equal to start time.", nameof(endTime));

        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeSlotDto" /> class.
    /// </summary>
    public TimeSlotDto()
    {
    }

    /// <summary>
    ///     Gets or sets the start time of the time slot.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    ///     Gets or sets the end time of the time slot.
    /// </summary>
    public DateTime EndTime { get; set; }
}