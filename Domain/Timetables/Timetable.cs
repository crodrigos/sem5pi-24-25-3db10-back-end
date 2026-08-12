using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.Timetables;

public class Timetable : Entity<Guid>, IAggregateRoot
{
    public LicenseNumber LicenseNumber { get; private set; }
    public DateTime DateShift { get; private set; }
    public TimeShift TimeShift { get; private set; }

    // Construtor protegido (para uso do ORM)
    protected Timetable() : base(Guid.NewGuid()) { }

    // Construtor com parâmetros para garantir a criação da entidade com os valores necessários
    public Timetable(Guid id, LicenseNumber licenseNumber, DateTime dateShift, TimeShift timeShift) 
        : base(id)
    {
        if (licenseNumber == null)
            throw new ArgumentNullException(nameof(licenseNumber), "License number cannot be null.");
        if (timeShift == null)
            throw new ArgumentNullException(nameof(timeShift), "Time shift cannot be null.");
        
        LicenseNumber = licenseNumber;
        DateShift = dateShift;
        TimeShift = timeShift;
    }

    // Método para atualizar o horário do turno (com validação, se necessário)
    public void UpdateTimeShift(TimeShift newTimeShift)
    {
        if (newTimeShift == null) throw new ArgumentNullException(nameof(newTimeShift), "New time shift cannot be null.");
        TimeShift = newTimeShift;
    }


    public override string ToString()
    {
        return $"Timetable for {LicenseNumber} on {DateShift:yyyy-MM-dd} from {TimeShift.Entrance:hh\\:mm} to {TimeShift.Exit:hh\\:mm}";
    }
}
