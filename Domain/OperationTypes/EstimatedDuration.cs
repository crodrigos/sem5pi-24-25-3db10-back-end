using dddnet8.Domain.Shared;

public class EstimatedDuration : ValueObject
{
    public TimeSpan AnesthesiaTimeDuration { get; private set; }
    public TimeSpan SurgeryDuration { get; private set; }
    public TimeSpan CleaningDuration { get; private set; }

    private double MinimumDurationExcluding = 0;

    public EstimatedDuration(TimeSpan anesthesiaTimeDuration, TimeSpan surgeryDuration, TimeSpan cleaningDuration)
    {
        if (anesthesiaTimeDuration.TotalMinutes <= MinimumDurationExcluding || 
            surgeryDuration.TotalMinutes <= MinimumDurationExcluding || 
            cleaningDuration.TotalMinutes <= MinimumDurationExcluding)
        {
            throw new ArgumentException("Duration cannot be negative or zero");
        }

        AnesthesiaTimeDuration = anesthesiaTimeDuration;
        SurgeryDuration = surgeryDuration;
        CleaningDuration = cleaningDuration;
    }

    // Sobrescrevendo o ToString para usar o formato "hh:mm:ss"
    public override string ToString()
    {
        return $"{AnesthesiaTimeDuration:hh\\:mm\\:ss},{SurgeryDuration:hh\\:mm\\:ss},{CleaningDuration:hh\\:mm\\:ss}";
    }

    // Método para reconstruir o EstimatedDuration a partir de uma string no formato "hh:mm:ss"
    public static EstimatedDuration FromString(string value)
    {
        var parts = value.Split(',');

        if (parts.Length != 3)
        {
            throw new ArgumentException("Invalid string format for EstimatedDuration");
        }

        // Parse cada parte da string como "hh:mm:ss"
        var anesthesiaTime = TimeSpan.ParseExact(parts[0], @"h\:mm\:ss", null);
        var surgeryTime = TimeSpan.ParseExact(parts[1], @"h\:mm\:ss", null);
        var cleaningTime = TimeSpan.ParseExact(parts[2], @"h\:mm\:ss", null);

        return new EstimatedDuration(anesthesiaTime, surgeryTime, cleaningTime);
    }
    
    public Double GetTotalMinutesEstimatedDuration()
    {
        // Somando as durações de anestesia, cirurgia e limpeza
        return AnesthesiaTimeDuration.TotalMinutes + SurgeryDuration.TotalMinutes + CleaningDuration.TotalMinutes;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return AnesthesiaTimeDuration;
        yield return SurgeryDuration;
        yield return CleaningDuration;
    }
}
