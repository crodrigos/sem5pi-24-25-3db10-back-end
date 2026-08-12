using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Staffs.V.O;

using System;
using System.Collections.Generic;

public class TimeShift : ValueObject
{
    // Properties to store the start and end times of the shift
    public TimeSpan Entrance { get; private set; }
    public TimeSpan Exit { get; private set; }

    // Minimum allowed duration for a shift
    private double MinimumDurationExcluding = 0;

    // Constructor to create a TimeShift object
    public TimeShift(TimeSpan entrance, TimeSpan exit)
    {
        // Validate that the exit time is not before the entrance time
        Entrance = entrance;
        Exit = exit;
    }

    // Override ToString method to format the time shift as "hh:mm:ss"
    public override string ToString()
    {
        // Convert the Entrance and Exit times to string in the "hh:mm:ss" format
        return $"{Entrance:hh\\:mm\\:ss},{Exit:hh\\:mm\\:ss}";
    }

    // Method to recreate a TimeShift object from a string in the "hh:mm:ss,hh:mm:ss" format
    public static TimeShift FromString(string value)
    {
        // Split the string by comma to separate the entrance and exit times
        var parts = value.Split(',');

        // Check if the input string has exactly two parts (entrance and exit times)
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid format for TimeShift. Expected format: hh:mm:ss,hh:mm:ss");
        }

        // Parse each part of the string as "hh:mm:ss"
        var entrance = TimeSpan.ParseExact(parts[0], @"h\:mm\:ss", null);
        var exit = TimeSpan.ParseExact(parts[1], @"h\:mm\:ss", null);

        // Create a TimeShift object
        return new TimeShift(entrance, exit);
    }

    // Override GetAtomicValues method from ValueObject base class to provide atomic values for comparisons and hashing
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Entrance;
        yield return Exit;
    }
}
