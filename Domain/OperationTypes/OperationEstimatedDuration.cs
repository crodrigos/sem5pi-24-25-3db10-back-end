using dddnet8.Domain.Shared;

namespace dddnet8.Domain.OperationTypes;

public class OperationEstimatedDuration : ValueObject
{
public TimeSpan AnesthesiaTimeDuration { get; private set; }
public TimeSpan SurgeryDuration { get; private set; }
public TimeSpan CleaningDuration { get; private set; }

private double MinimumDurationExcluding = 0;

public OperationEstimatedDuration(TimeSpan anesthesiaTimeDuration, TimeSpan surgeryDuration, TimeSpan cleaningDuration)
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
    
protected override IEnumerable<object> GetAtomicValues()
{
    // Retorna os valores atômicos (propriedades) da classe
    yield return AnesthesiaTimeDuration;
    yield return SurgeryDuration;
    yield return CleaningDuration;
}
}