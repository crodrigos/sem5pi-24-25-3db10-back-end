using dddnet8.Domain.Appointments.V.O;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.Shared;
using dddnet8.Domain.SurgeryRooms;
using dddnet8.Domain.SurgeryRooms.V.O;

namespace dddnet8.Domain.Appointments.Entities;

public class Appointment : Entity<Guid>, IAggregateRoot
{
    protected Appointment() : base(Guid.NewGuid()) {}
    
    public Appointment(OperationRequestCode operationRequest, RoomNumber surgeryRoom, DateOnly appointmentDate) : base(Guid.NewGuid())
    {
        OperationRequest = operationRequest;
        SurgeryRoom = surgeryRoom;
        AppointmentStatus = AppointmentStatus.Scheduled;
        AppointmentDate = appointmentDate;
    }
    public OperationRequestCode OperationRequest { get; private set; }
    public RoomNumber SurgeryRoom { get; private set; }
    public AppointmentStatus AppointmentStatus { get; private set; }
    public DateOnly AppointmentDate { get; private set; }


    public void UpdateStatus(AppointmentStatus status)
    {
        if (status == null)
        {
            throw new ArgumentNullException(nameof(status), "Status cannot be null.");
        }

        if (AppointmentStatus == status)
        {
            throw new InvalidOperationException($"The appointment is already in the '{status}' status.");
        }

        // Add any domain-specific validation here if needed.
        // Example: Prevent certain transitions if business logic dictates
        if (AppointmentStatus == AppointmentStatus.Completed && status != AppointmentStatus.Canceled)
        {
            throw new InvalidOperationException("Cannot update the status of a completed appointment unless it is being cancelled.");
        }

        AppointmentStatus = status;
    }

    public void UpdateSurgeryRoom(RoomNumber newRoom)
    {
        if (newRoom == null)
        {
            throw new ArgumentNullException(nameof(newRoom), "New room number cannot be null.");
        }

        SurgeryRoom = newRoom;
    }

    public void UpdateDate(DateOnly newDate)
    {
        if (newDate == null)
        {
            throw new ArgumentNullException(nameof(newDate), "New appointment date cannot be null.");
        }

        if (newDate < AppointmentDate)
        {
            throw new InvalidOperationException("Cannot update the appointment date to a date in the past.");
        }

        AppointmentDate = newDate;
    }
}