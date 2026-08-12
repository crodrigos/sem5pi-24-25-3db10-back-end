using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;
using SurgicalManagement.Domain.Common;
using YourNamespace.GDPR.Entities;

namespace dddnet8.AuditLog.Entities
{
    public class OperationRequestLog : LogEntry
    {
        public Guid OperationRequestId { get; private set; }
        public MedicalRecordNumber PatientId { get; private set; }
        public LicenseNumber DoctorId { get; private set; }
        public OperationTypeCode OperationTypeId { get; private set; }
        public DateTime DeadlineDate { get; private set; }
        public OperationRequestPriority Priority { get; private set; }
        public string OperationDescription { get; private set; }
        public OperationRequestStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }
        public bool IsScheduled { get; private set; }

        // Protected constructor for EF (if necessary)
        protected OperationRequestLog() : base("action", "OperationRequest") { }

        public OperationRequestLog(string action, OperationRequest operationRequest)
            : base(action, "OperationRequest")
        {
            OperationRequestId = operationRequest.Id;
            PatientId = operationRequest.PatientId;
            DoctorId = operationRequest.DoctorId;
            OperationTypeId = operationRequest.OperationTypeId;
            DeadlineDate = operationRequest.DeadlineDate;
            Priority = operationRequest.Priority;
            OperationDescription = operationRequest.OperationDescription.Value; // OperationDescription is a Value Object
            Status = operationRequest.Status;
            CreatedDate = operationRequest.CreatedDate;
            LastUpdatedDate = operationRequest.LastUpdatedDate;
            IsScheduled = operationRequest.IsScheduled;
        }
    }
}