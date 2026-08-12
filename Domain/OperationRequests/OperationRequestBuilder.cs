using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.Staffs.V.O;

namespace dddnet8.Domain.OperationRequests;

public class OperationRequestBuilder
{
    
    private IOperationRequestCodeGenerator _operationRequestCodeGenerator;
    
    private MedicalRecordNumber _patientId;
    private LicenseNumber _doctorId;
    private OperationTypeCode _operationTypeId;
    private DateTime _deadlineDate;
    private OperationRequestPriority _priority;
    private string _description;
    private OperationRequestStatus _status;
    private OperationType _operationType;
    public OperationRequestCode _operationRequestCode { get; private set; }
    
    public OperationRequestBuilder(IOperationRequestCodeGenerator operationRequestCodeGenerator)
    {
        _operationRequestCodeGenerator = operationRequestCodeGenerator;
    }

    public OperationRequestBuilder WithPatientId(MedicalRecordNumber patientId)
    {
        _patientId = patientId;
        return this;
    }

    public OperationRequestBuilder WithDoctorId(LicenseNumber doctorId)
    {
        _doctorId = doctorId;
        return this;
    }

    public OperationRequestBuilder WithOperationType(OperationTypeCode operationTypeId)
    {
        _operationTypeId = operationTypeId;
        return this;
    }

    public OperationRequestBuilder WithDeadlineDate(DateTime deadlineDate)
    {
        _deadlineDate = deadlineDate;
        return this;
    }

    public OperationRequestBuilder WithPriority(OperationRequestPriority priority)
    {
        _priority = priority;
        return this;
    }

    public OperationRequestBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public OperationRequestBuilder WithStatus(OperationRequestStatus status)
    {
        _status = status;
        return this;
    }

    public async Task<OperationRequest> Build()
    {

        if (_operationRequestCode == null)
        {
            _operationRequestCode = await _operationRequestCodeGenerator.GenerateOperationRequestCode();
        }

        var operationRequest = OperationRequest.Create(
            _patientId,
            _doctorId,
            _operationTypeId,
            _deadlineDate,
            _priority,
            _description,
            _operationRequestCode
        );

        if (operationRequest.IsFailure)
        {
            throw new InvalidOperationException("Failed to create operation request.");
        }
        
        return operationRequest.Value;
    }
}