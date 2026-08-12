using System;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations;
using dddnet8.Infraestructure.RequiredStaffs;
using dddnet8.Infraestructure.Shared;
using dddnet8.Infraestructure.Shared.Exceptions;
using Name = dddnet8.Domain.OperationTypes.Names.Name;

namespace dddnet8.Infraestructure.OperationTypes;

public class OperationTypeService : IOperationTypeService
{
    
    private readonly IOperationTypeRepository _operationTypeRepository;
    private readonly IRequiredStaffRepository _requiredStaffRepository;
    private readonly IOperationTypeCodeGenerator _operationTypeCodeGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public OperationTypeService(IOperationTypeRepository operationTypeRepo, IRequiredStaffRepository requiredStaffRepository,
        IOperationTypeCodeGenerator operationTypeCodeGenerator, IUnitOfWork unitOfWork)
    {
        this._operationTypeRepository = operationTypeRepo ?? throw new ArgumentNullException(nameof(operationTypeRepo));
        this._requiredStaffRepository = requiredStaffRepository ?? throw new ArgumentNullException(nameof(requiredStaffRepository));
        this._operationTypeCodeGenerator = operationTypeCodeGenerator ?? throw new ArgumentNullException(nameof(operationTypeCodeGenerator));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    
    public async Task<List<OperationTypeDTO>> GetAll()
    {
        List<OperationType> all = await _operationTypeRepository.GetAllOperationTypesAsync();
        List<OperationTypeDTO> dtos = new List<OperationTypeDTO>();

        foreach (var operationType in all)
        {
            var reqStaff = await _requiredStaffRepository.GetByOperationTypeAsync(operationType);
            if (reqStaff != null)
            {
                dtos.Add(OperationTypeDTOMapper.ToDTO(operationType, reqStaff));
            }
        }

        return dtos;
    }

    public async Task<List<OperationTypeDTO>> GetByStatus(int status)
    {

        Status statusEnum = Status.Active;
        switch (status)
        {
            case 0:
                statusEnum = Status.Inactive;
                break;
            case 1:
                statusEnum = Status.Active;
                break;
        }

        List<OperationType> all = await _operationTypeRepository.GetByStatusAsync(statusEnum);
        
        var dtos = new List<OperationTypeDTO>();
        
        if (all != null)
        {
            all.ForEach(async operationType => {
                var reqStaff = await _requiredStaffRepository.GetByOperationTypeAsync(operationType);
                if (reqStaff != null) {
                    dtos.Add(OperationTypeDTOMapper.ToDTO(operationType, reqStaff));
                }
            });
        }

        return dtos;

    }

    public async Task<OperationTypeDTO> GetById(string id)
    {
        // Check if id is a valid Guid
        if (!Guid.TryParse(id, out Guid guid))
        {
            throw new ArgumentException("Given id is not a valid Guid.");
        }

        OperationType operationType = await _operationTypeRepository.GetByIdAsync(guid);
        
        var reqStaff = await _requiredStaffRepository.GetByOperationTypeAsync(operationType);
        
        return OperationTypeDTOMapper.ToDTO(operationType, reqStaff);
    }

    public static ValidationResult validateDTO(OperationTypeDTO operationTypeDTO)
    {
        ValidationResult validationResult = new ValidationResult();

        // Validate if Name is valid
        try {
            Name name = new Name(operationTypeDTO.Name);
        } catch (ArgumentException e) {
            validationResult.AddErrorMessage(e.Message);
        }

        // Validate if Status is valid
        if (!Enum.IsDefined(typeof(Status), operationTypeDTO.Status))
        {
            validationResult.AddErrorMessage("Status is not a valid status.");
        }
        
        // Validate if EstimatedDuration is valid
        try {
            EstimatedDuration estimatedDuration = new EstimatedDuration(new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0),new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0));
        } catch (ArgumentException e) {
            validationResult.AddErrorMessage(e.Message);
        }

        // Validate if all required staff are valid
        operationTypeDTO.RequiredStaff.ForEach(requiredStaffDTO => {    
            try {
                Specialization specialization = new Specialization(Domain.Patients.V.O.Name.Create(requiredStaffDTO.SpecializationName), Description.Create(requiredStaffDTO.SpecializationDescription).Value, SpecializationCode.Create(requiredStaffDTO.SpecializationCode));
            } catch (ArgumentException e) {
                validationResult.AddErrorMessage(e.Message);
            }

            try {
                RequiredStaffQuantity quantity = new RequiredStaffQuantity(requiredStaffDTO.Quantity);
            } catch (ArgumentException e) {
                validationResult.AddErrorMessage(e.Message);
            }
        });

        return validationResult;
    }

    public static ValidationResult verifyOperationTypeAdd(OperationTypeAddDTO operationTypeDTO)
    {
        ValidationResult validationResult = new ValidationResult();

        // Validate if Name is valid
        try {
            Name name = new Name(operationTypeDTO.Name);
        } catch (ArgumentException e) {
            validationResult.AddErrorMessage(e.Message);
        }

        // Validate if Status is valid
        if (!Enum.IsDefined(typeof(Status), operationTypeDTO.Status))
        {
            validationResult.AddErrorMessage("Status is not a valid status.");
        }
        
        // Validate if EstimatedDuration is valid
        try {
            EstimatedDuration estimatedDuration = new EstimatedDuration(new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0));
        } catch (ArgumentException e) {
            validationResult.AddErrorMessage(e.Message);
        }

        operationTypeDTO.RequiredStaff.ForEach(requiredStaffDTO => {    
            try {
                
                Specialization specialization = new Specialization(Domain.Patients.V.O.Name.Create(requiredStaffDTO.SpecializationName), Description.Create(requiredStaffDTO.SpecializationDescription).Value, SpecializationCode.Create(requiredStaffDTO.SpecializationCode));
            } catch (ArgumentException e) {
                validationResult.AddErrorMessage(e.Message);
            }

            try {
                RequiredStaffQuantity quantity = new RequiredStaffQuantity(requiredStaffDTO.Quantity);
            } catch (ArgumentException e) {
                validationResult.AddErrorMessage(e.Message);
            }
        });

        return validationResult;
    }
    
    public async Task<OperationTypeDTO> Add(OperationTypeAddDTO operationTypeDTO)
    { 
        
        ValidationResult validationResult = verifyOperationTypeAdd(operationTypeDTO);

        if (!validationResult.IsValid)
        {
            throw new MultipleArgumentException(validationResult.ErrorMessages.ToArray());
        }
        
        Guid id = Guid.NewGuid();
        Name name = new Name(operationTypeDTO.Name);
        Status status = (Status) operationTypeDTO.Status;
        // TODO -> RETIFICAR
        EstimatedDuration estimatedDuration = new EstimatedDuration(new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0));
        
        OperationTypeCode operationCode = _operationTypeCodeGenerator.GenerateOperationCode();

        OperationType operationType = new OperationType(id, name, status, estimatedDuration, operationCode);

        List<RequiredStaff> requiredStaffs = operationTypeDTO.RequiredStaff.Select(requiredStaffDTO => {

            Guid requiredStaffId = Guid.NewGuid();
            Specialization specialization = new Specialization(Domain.Patients.V.O.Name.Create(requiredStaffDTO.SpecializationName), Description.Create(requiredStaffDTO.SpecializationDescription).Value, SpecializationCode.Create(requiredStaffDTO.SpecializationCode));
            RequiredStaffQuantity quantity = new RequiredStaffQuantity(requiredStaffDTO.Quantity);

            return new RequiredStaff(requiredStaffId, specialization, quantity, operationType);

        }).ToList();

        var resultOperationType = await _operationTypeRepository.AddOperationType(operationType);
        
        List<RequiredStaff> resultRequiredStaffs = new List<RequiredStaff>();
        foreach (var requiredStaff in requiredStaffs)
        {
            var addedRequiredStaff = await _requiredStaffRepository.AddAsync(requiredStaff);
            resultRequiredStaffs.Add(addedRequiredStaff);
            await _requiredStaffRepository.Save();
        }

        _unitOfWork.CommitAsync();
        
        return OperationTypeDTOMapper.ToDTO(resultOperationType, resultRequiredStaffs);
    }

    public async Task<OperationTypeDTO> Update(string id, OperationTypeDTO operationTypeDTO)
    {
        ValidationResult validationResult = validateDTO(operationTypeDTO);

        if (!validationResult.IsValid)
        {
            throw new MultipleArgumentException(validationResult.ErrorMessages.ToArray());
        }
        
        Guid guid = Guid.Parse(operationTypeDTO.Id);
        OperationType existingOperationType = await _operationTypeRepository.GetByIdAsync(guid);

        if (existingOperationType == null)
        {
            throw new ArgumentException("OperationType not found.");
        }

        Name name = new Name(operationTypeDTO.Name);
        Status status = (Status)operationTypeDTO.Status;
        EstimatedDuration estimatedDuration = new EstimatedDuration(new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0), new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0));

        existingOperationType.Name = name;
        existingOperationType.Status = status;
        existingOperationType.EstimatedDuration = estimatedDuration;
        
        List<RequiredStaff> requiredStaffs = operationTypeDTO.RequiredStaff.Select(requiredStaffDTO => {
            Guid requiredStaffId = Guid.Parse(requiredStaffDTO.Id);
            Specialization specialization = new Specialization(Domain.Patients.V.O.Name.Create(requiredStaffDTO.SpecializationName), Description.Create(requiredStaffDTO.SpecializationDescription).Value, SpecializationCode.Create(requiredStaffDTO.SpecializationCode));
            RequiredStaffQuantity quantity = new RequiredStaffQuantity(requiredStaffDTO.Quantity);

            return new RequiredStaff(requiredStaffId, specialization, quantity, existingOperationType);
        }).ToList();

        var updatedOperationType = await _operationTypeRepository.UpdateAsync(existingOperationType);

        await _requiredStaffRepository.RemoveByOperationType(existingOperationType);
        List<RequiredStaff> updatedRequiredStaffs = new List<RequiredStaff>();
        requiredStaffs.ForEach(async requiredStaff => {
            updatedRequiredStaffs.Add(await _requiredStaffRepository.AddAsync(requiredStaff));
        });

        _unitOfWork.CommitAsync();
        
        return OperationTypeDTOMapper.ToDTO(updatedOperationType, updatedRequiredStaffs);
    }

        
    
    public async Task<OperationTypeDTO> RemoveByCode(string code)
    {

        OperationTypeCode operationCode;
        try
        {
            operationCode = OperationTypeCode.Create(code);
        }
        catch (Exception e)
        {
            throw new Exception("Operation Type Code is not valid", e);
        }

        OperationType operationType = await _operationTypeRepository.GetByOperationTypeCode(operationCode);
        if (operationType == null)
        {
            throw new ArgumentException($"OperationType with code {code} not found.");
        }
        
        List<RequiredStaff> requiredStaffs = await _requiredStaffRepository.GetByOperationTypeAsync(operationType);
        
        OperationTypeDTO dto = OperationTypeDTOMapper.ToDTO(operationType, requiredStaffs);
        
        try {
            await _requiredStaffRepository.RemoveByOperationType(operationType);
            _operationTypeRepository.Remove(operationType);

            await _unitOfWork.CommitAsync();
            
            return dto; 
        } catch (Exception e) {
            throw new Exception("Error removing OperationType.", e);
        }
    }
}
