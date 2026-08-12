using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Infraestructure.Shared;

namespace dddnet8.Domain.OperationRequests;

public class OperationRequest : Entity<Guid>
{
    public MedicalRecordNumber PatientId { get; private set; }
    public LicenseNumber DoctorId { get; private set; } // Staff.Role = Doctor
    
    public OperationRequestCode OperationRequestCode { get; private set; }
    public OperationTypeCode OperationTypeId { get; private set; }
    public DateTime DeadlineDate { get; private set; }
    public OperationRequestPriority Priority { get; private set; }
    public Description OperationDescription { get; private set; }
    public OperationRequestStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime LastUpdatedDate { get; private set; }
    public bool IsScheduled { get; private set; }

    // Private constructor for EF
    private OperationRequest() : base(Guid.NewGuid())
    {
        Status = OperationRequestStatus.Pending; // Default status
        CreatedDate = DateTime.UtcNow; // Set created date
        LastUpdatedDate = DateTime.UtcNow; // Set last updated date
        IsScheduled = false;
    }

    // Factory method to create an OperationRequest
    public static Result<OperationRequest> Create(
        MedicalRecordNumber patientId,
        LicenseNumber doctorId,
        OperationTypeCode operationTypeId,
        DateTime deadlineDate,
        OperationRequestPriority priority,
        string descriptionText,
        OperationRequestCode operationRequestCode
        
        )
    {
        // Validate deadline date
        if (deadlineDate < DateTime.UtcNow)
        {
            return "Deadline date cannot be in the past.";
        }

        // Validate and create description
        var description = Description.Create(descriptionText);
        if (description.IsFailure)
        {
            return description.Error;
        }

        // Create OperationRequest instance
        var operationRequest = new OperationRequest
        {
            PatientId = patientId,
            DoctorId = doctorId,
            OperationTypeId = operationTypeId,
            DeadlineDate = deadlineDate,
            Priority = priority,
            OperationDescription = description.Value,
            OperationRequestCode = operationRequestCode
        };

        return operationRequest;
    }
    
    // Second factory method for convinience
    /*public static Result<OperationRequest> Create(CreateOperationRequestDto dto)
    {
        if (dto == null)
        {
            return Result<OperationRequest>.Err("CreateOperationRequestDto cannot be null.");
        }
        return Create(dto.PatientId, dto.DoctorId, dto.OperationTypeId, dto.DeadlineDate, dto.Priority, dto.Description);
    }*/

    // AC: Doctors can update operation requests they created (e.g., change the deadline or priority).
    public void UpdateDeadline(DateTime deadlineDate)
    {
        DeadlineDate = deadlineDate;
        LastUpdatedDate = DateTime.UtcNow;
    }

    public void UpdatePriority(OperationRequestPriority priority)
    {
        Priority = priority;
        LastUpdatedDate = DateTime.UtcNow;
    }
    
    public void UpdateStatus(OperationRequestStatus result)
    {
        Status = result;
        LastUpdatedDate = DateTime.UtcNow;
    }
    

    public void UpdateDescription(string description)
    {
        var voDescription = Description.Create(description);
        if (voDescription.IsFailure)
        {
            return; // Error handling
        }

        OperationDescription = voDescription.Value;
        LastUpdatedDate = DateTime.UtcNow;
    }

    // Business behavior to change the status with validation
    public void Approve()
    {
        ChangeStatus(OperationRequestStatus.Approved);
    }

    public void Reject()
    {
        ChangeStatus(OperationRequestStatus.Rejected);
    }

    // Private helper to ensure status change rules
    private void ChangeStatus(OperationRequestStatus newStatus)
    {
        if (Status != OperationRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved or rejected.");

        Status = newStatus;
        LastUpdatedDate = DateTime.UtcNow;
    }

    public void MarkAsScheduled()
    {
        if (IsScheduled)
            throw new InvalidOperationException("The operation has already been scheduled.");

        IsScheduled = true;
        LastUpdatedDate = DateTime.UtcNow;
    }

    public bool HasBeenScheduled() => IsScheduled;


    
}