using App.Onion.Domain.Interfaces.PatientRepository;
using AutoMapper;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.PlanningModuleNotifications;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.Staffs.V.O;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace dddnet8.Infraestructure.OperationRequests;

public class OperationRequestService : IOperationRequestService
{
    private readonly IOperationRequestPolicy _operationRequestPolicy;
    private readonly IPlanningModuleNotificationService _planningModuleNotificationService;
    private readonly ILogService<OperationRequest> _logService;
    private readonly IStaffRepository _staffRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IOperationRequestRepository _operationRequestRepository;
    private readonly IOperationTypeRepository _operationTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OperationRequestService> _logger;
    private readonly IOperationRequestCodeGenerator _operationRequestCodeGenerator;

    public OperationRequestService(
        IOperationRequestPolicy operationRequestPolicy,
        IPlanningModuleNotificationService planningModuleNotificationService,
        ILogService<OperationRequest> logService,
        IStaffRepository staffRepository,
        IPatientRepository patientRepository,
        IOperationRequestRepository operationRequestRepository,
        IOperationTypeRepository operationTypeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<OperationRequestService> logger,
        IOperationRequestCodeGenerator operationRequestCodeGenerator)
    {
        _operationRequestPolicy = operationRequestPolicy;
        _planningModuleNotificationService = planningModuleNotificationService;
        _logService = logService;
        _staffRepository = staffRepository;
        _patientRepository = patientRepository;
        _operationRequestRepository = operationRequestRepository;
        _operationTypeRepository = operationTypeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _operationRequestCodeGenerator = operationRequestCodeGenerator;
    }

    // -----------------------------------------------------------------------------------------------------------------
    public async Task<OperationRequestDto> CreateOperationRequest(CreateOperationRequestDto dto)
    {

        if (!CheckIfDoctorExists(dto.DoctorId).Result) {throw new KeyNotFoundException($"Doctor with {dto.DoctorId} does not exist.");}

        if (!checkIfPatientExists(dto.PatientId).Result) {throw new KeyNotFoundException($"Patient with ${dto.PatientId} does not exist.");}

        var (isOpTypeValid, OpType) = CheckIfOperationTypeExists(dto.OperationTypeId).Result;

        if (!isOpTypeValid) {throw new KeyNotFoundException($"Operation type with ${dto.OperationTypeId} does not exist.");}

        var (isValid, priority) = CheckIfPriorityExists(dto.Priority);

        if (!isValid) {throw new KeyNotFoundException($"Priority with {dto.Priority} does not exist.");}

       // if (!CheckIfDoctorHasSpecializationForOperationType(dto.DoctorId, dto.OperationTypeId)) {throw new KeyNotFoundException("Doctor does not have the required specialization for this Operation.");}

        var mrn = MedicalRecordNumber.Create(dto.PatientId);
        
        var licenseNumber = new LicenseNumber(dto.DoctorId);

        if (!await _operationRequestPolicy.CanCreateRequest(mrn, licenseNumber)) {throw new ArgumentException("Operation Request could not be created due to policy. Please enter contact with administrator.");}

        var operationRequest = await new OperationRequestBuilder(_operationRequestCodeGenerator)
            .WithPatientId(mrn)
            .WithDoctorId(licenseNumber)
            .WithPriority((OperationRequestPriority) priority!)
            .WithDescription(dto.Description)
            .WithDeadlineDate(dto.DeadlineDate)
            .WithOperationType(OpType!)
            .Build();
        
        await _operationRequestRepository.AddOperationRequestAsync(operationRequest);

        return _mapper.Map<OperationRequestDto>(operationRequest);
    }

    
    private bool CheckIfDoctorHasSpecializationForOperationType(string doctorId, string operationTypeId)
    {
        var doctor = getDoctorLicenseNumber(doctorId).Result;
        if (doctor == null) {throw new Exception("Doctor is Null in checkIfDoctorHasSpecializationForOperationType. Therefore, doctor does not exist.");}
        
        var operationType = GetOperationType(operationTypeId).Result;
        if (operationType == null) {throw new Exception("Operation Type is Null in checkIfDoctorHasSpecializationForOperationType. Therefore, Operation type does not exist.");}

        return doctor.HasSpecializationForOperationType(operationType);

    }

    private (bool isValid, OperationRequestPriority? priority) CheckIfPriorityExists(string dtoPriority)
    {
        var isValid = Enum.TryParse<OperationRequestPriority>(dtoPriority, out var priority);
        return (isValid, isValid ? priority : null);
    }

    private async Task<(bool exists, OperationTypeCode? code)> CheckIfOperationTypeExists(string dtoOperationTypeId)
    {
        var operationType = await GetOperationType(dtoOperationTypeId);
    
        if (operationType != null)
        {
            return (true, operationType.OperationTypeCode); // Retorna true e o código.
        }
    
        return (false, null); 
    }

    private async Task<OperationType?> GetOperationType(string dtoOperationTypeId)
    {
        var opCode = OperationTypeCode.Create(dtoOperationTypeId);
        
      var opType =  await _operationTypeRepository.GetByOperationTypeCode(opCode);
        
      return opType;
    }

    private async Task<bool> checkIfPatientExists(string dtoPatientId)
    {
        var mrn = MedicalRecordNumber.Create(dtoPatientId);
        var patientDataModel = await _patientRepository.GetPatientByMedicalRecordNumber(mrn);
        return patientDataModel != null;
    }

    private async Task<bool> CheckIfDoctorExists(string dtoDoctorId)
    {
        var doctorStaff = await getDoctorLicenseNumber(dtoDoctorId);
        return doctorStaff != null;
    }

    private async Task<Domain.Staffs.Staff?> getDoctorLicenseNumber(string dtoDoctorId)
    {
        var licenseNumber = new LicenseNumber(dtoDoctorId);
        return await _staffRepository.GetByLicenseNumberAsync(licenseNumber);
    }


    // -----------------------------------------------------------------------------------------------------------------
    public async Task<Result<OperationRequestDto>> GetOperationRequest(Guid id)
    {
        var operationRequest = await _operationRequestRepository.GetByIdAsync(id);
        if (operationRequest == null)
            return $"The operation request with id: {id} was not found";

        return _mapper.Map<OperationRequestDto>(operationRequest);
    }

    // -----------------------------------------------------------------------------------------------------------------
    public async Task<Result<List<GetAllOperationRequestsDto>>> GetAllOperationRequests()
    {
        var operationRequests = await _operationRequestRepository.GetAllAsync();

        var operationRequestsDto = operationRequests.Select(or => new GetAllOperationRequestsDto
        {
            PatientId = or.PatientId.Value,
            DoctorId = or.DoctorId.Value,
            OperationTypeId = or.OperationTypeId._OperationTypeCode,
            OperationRequestCode = or.OperationRequestCode._operationRequestCode
        }).ToList();
        // TODO -----> CASO QUEIRAS TROCAR JB
        
        return Result<List<GetAllOperationRequestsDto>>.Ok(operationRequestsDto);
    }


    // -----------------------------------------------------------------------------------------------------------------
    public async Task<OperationRequestDto> UpdateOperationRequest(OperationRequestCriteria dto, string id)
    {

        var operationRequest = await _operationRequestRepository.GetByOperationRequestCode(id);
        
        if (operationRequest == null) {throw new KeyNotFoundException("Operation Request does not exist");}
        
        //if (operationRequest.DoctorId.Value != dto.DoctorId) {throw new ArgumentException("You are not authorized to update this operation request.");}

        if (dto.Deadline != null) {operationRequest.UpdateDeadline((DateTime)dto.Deadline);}

        if (dto.Priority != null)
        {
            if (Enum.TryParse<OperationRequestPriority>(dto.Priority, true, out var priority))
            {
                operationRequest.UpdatePriority(priority);
            }
            else
            {
                throw new NotImplementedException($"Priority {dto.Priority} is not implemented.");
            }
        }
        
        if (dto.Status != null)
        {
            if (Enum.TryParse<OperationRequestStatus>(dto.Status, true, out var status))
            {
                operationRequest.UpdateStatus(status);
            }
            else
            {
                throw new NotImplementedException($"Status {dto.Status} is not implemented.");
            }
        }

        

        await _unitOfWork.CommitAsync();

        // AC: Updated requests are reflected immediately in the system and notify the Planning Module of any changes.
        //await _planningModuleNotificationService.NotifyAsync(
          //  PlanningModuleNotificationMessages.OperationRequestUpdated(id));

        // AC: The system logs all updates to the operation request (e.g., changes to priority or deadline).
        //await _logService.LogActionAsync("Update", operationRequest);

        return _mapper.Map<OperationRequestDto>(operationRequest);
    }

    // -----------------------------------------------------------------------------------------------------------------
    public async Task<Result<string>> DeleteOperationRequest(string id)
    {
        var operationRequest = await _operationRequestRepository.GetByOperationRequestCode(id);
        
        Console.WriteLine("SERVICE ->" + operationRequest.OperationRequestCode);

        if (operationRequest == null)
            return Result<string>.Err($"Operation request with id: {id} was not found.");

        // AC: Doctors can only delete operation requests they created if the operation has not yet been scheduled.
        if (operationRequest.HasBeenScheduled())
            return Result<string>.Err($"Operation request with id: {id} cannot be removed because it has already been scheduled.");
        
        await _operationRequestRepository.RemoveOperationRequest(operationRequest);

        // AC: Notify the planning module and update any schedules that were using this request. TODO: Sprint2
        //await _planningModuleNotificationService.NotifyAsync(
        //PlanningModuleNotificationMessages.OperationRequestDeleted(id));

        return Result<string>.Ok(id);
    }

    public async Task<Result<List<OperationRequestDTOV2>>> SearchOperationRequests(OperationRequestCriteria criteria)
    {
        var operationRequests = await _operationRequestRepository.SearchOperationRequestsByFiltersAsync(criteria);

        if (!operationRequests.Any())
        {
            return "No operation requests found matching the criteria.";
        }

        return _mapper.Map<List<OperationRequestDTOV2>>(operationRequests);
    }
}