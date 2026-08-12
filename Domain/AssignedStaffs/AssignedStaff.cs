using dddnet8.Domain.Appointments.Entities;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;

using dddnet8.Domain.AssignedStaff;
public class AssignedStaff : Entity<Guid>, IAggregateRoot
{
    // Construtor protegido para uso em frameworks ORM
    protected AssignedStaff() : base(Guid.NewGuid()) {}

    // Construtor que associa o staff a um appointment
    public AssignedStaff(Guid appointmentId, LicenseNumber assignedLicenseNumber) : base(Guid.NewGuid())
    {
        if (assignedLicenseNumber == null)
            throw new ArgumentNullException(nameof(assignedLicenseNumber), "License number cannot be null.");

        AppointmentId = appointmentId;
        AssignedLicenseNumber = assignedLicenseNumber;
    }

    // ID do Appointment ao qual este staff foi atribuído
    public Guid AppointmentId { get; private set; }

    // Número de licença do staff atribuído
    public LicenseNumber AssignedLicenseNumber { get; private set; }

    // Propriedade de navegação para o Appointment (opcional, caso queira carregar o Appointment completo)
    public virtual Appointment Appointment { get; private set; }
}