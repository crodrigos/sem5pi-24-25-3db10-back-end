using App.Onion.Application.Dtos;
using App.Onion.Application.Interfaces;
using App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;
using App.Onion.Domain.Interfaces.PatientRepository;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;
using dddnet8.Infraestructure.OperationRequests;


public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMedicalRecordNumberGenerator _medicalRecordNumberGenerator;
    private readonly IEmailService _emailService;
    private readonly ISystemUserService _systemUserService;

    private IPatientService _patientServiceImplementation;
    private readonly ILogService<Patient> _patientLogService;

    public PatientService(IPatientRepository patientRepository,
        IMedicalRecordNumberGenerator medicalRecordNumberGenerator, ISystemUserService systemUserService,
        IEmailService emailService, ILogService<Patient> patientLogService)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _medicalRecordNumberGenerator = medicalRecordNumberGenerator ??
                                        throw new ArgumentNullException(nameof(medicalRecordNumberGenerator));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _patientLogService = patientLogService ?? throw new ArgumentNullException(nameof(patientLogService));
        _systemUserService = systemUserService ?? throw new ArgumentNullException(nameof(systemUserService));
    }

    public async Task<PatientDto> CreatePatient(CreatePatientDTO patientDto)
    {

        if (!Enum.TryParse<Gender>(patientDto.Gender, out var gender))
        {
            throw new ArgumentException("Invalid gender");
        }

        var patient = await new PatientBuilder(_medicalRecordNumberGenerator)
            .WithFirstName(patientDto.FirstName).WithLastName(patientDto.LastName)
            .WithGender(gender)
            .WithDateOfBirth(DateOfBirth.Create(patientDto.DateOfBirth))
            .WithContactInformation(patientDto.ContactInformation)
            .WithEmergencyContact(patientDto.EmergencyContact).Build();

        await _patientRepository.AddPatientAsync(patient);

        return PatientMapper.ToDto(patient);
    }

    public async Task<IEnumerable<PatientDto>?> SearchPatientsByFilters(PatientCriteria criteria)
    {
        try
        {
            var patients = await _patientRepository.SearchPatientsByFiltersAsync(criteria);

            var patientDtoList = patients.Select(PatientMapper.ToDto).ToList();

            return patientDtoList;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PatientDto> UpdatePatientData(PatientCriteria patientcriteria, string medicalRecordNumber)
    {
        try
        {
            var patient = await GetPatientByMedicalRecordNumber(medicalRecordNumber);

            patient.UpdatePatient(patientcriteria);

            return await UpdatePatient(patient, patientcriteria);
        }
        catch (KeyNotFoundException ex)
        {
            throw new Exception("Patient not found.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<PatientDto> UpdatePatient(Patient patient, PatientCriteria patientcriteria)
    {
        try
        {
            var patientEmail = patient.ContactInformation.EmailAddress;

            await UpdatePatientInRepository(patient);

            await _patientLogService.LogActionAsync("Update", patient);

            await NotifyPatientIfContactUpdated(patientEmail, patientcriteria);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return PatientMapper.ToDto(patient);
    }

    public async Task<(bool result, string message)> ValidatePatientForDeletion(string medicalRecordNumber)
    {
        try
        {
            var patient = await GetPatientByMedicalRecordNumber(medicalRecordNumber);

            if (patient == null)
            {
                return (false, "Patient not found.");
            }

            return patient.DeletionStatus.IsToDelete
                ? (false, $"Patient is already marked for deletion.{patient.DeletionStatus.DeletionDate.Value}")
                : (true,
                    $"Patient {patient.FullName} is eligible for deletion. Are you sure you want to mark this patient to be deleted?");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred: {ex.Message}");
        }
    }

    public async Task MarkPatientForDeletion(string medicalRecordNumber)
    {
        try
        {
            var patient = await GetPatientByMedicalRecordNumber(medicalRecordNumber);

            patient.MarkForDeletion();

            await UpdatePatientInRepository(patient);
        }
        catch (KeyNotFoundException ex)
        {
            throw new Exception("Patient not found.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while marking the patient for deletion.", ex);
        }
    }

    private async Task<Patient> GetPatientByMedicalRecordNumber(string medicalRecordNumber)
    {
        var patient =
            await _patientRepository.GetPatientByMedicalRecordNumber(MedicalRecordNumber.Create(medicalRecordNumber));
        if (patient == null)
        {
            throw new KeyNotFoundException("Patient not found");
        }

        return patient;
    }

    private async Task NotifyPatientIfContactUpdated(EmailAddress patientEmailAddress, PatientCriteria patientcriteria)
    {
        if (patientcriteria.EmergencyContact != null || patientcriteria.ContactInformation != null)
        {
            await _emailService.NotifyClientAboutUpdate(patientEmailAddress);
        }
    }

    private async Task UpdatePatientInRepository(Patient patient)
    {
        await _patientRepository.UpdatePatientDataAsync(patient);
    }

    public async Task MarkPatientAssociatedDataForDeletion(string medicalRecordNumber)
    {
        try
        {
            var patient = await GetPatientByMedicalRecordNumber(medicalRecordNumber);

            //await _systemUserService.MarkUserForDeletion(patient.ContactInformation.EmailAddress.ToString());

            Console.WriteLine(patient.FullName);

            patient.MarkForDeletion();

            await UpdatePatientInRepository(patient);
        }
        catch (KeyNotFoundException ex)
        {
            throw new Exception("Patient not found.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PatientDto> GetPatientByUsername(string username)
    {
        try
        {
            var patient = await _patientRepository.GetPatientByEmailAddress(EmailAddress.Create(username));

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            return PatientMapper.ToDto(patient);
        }
        catch (KeyNotFoundException ex)
        {
            throw new Exception("Patient not found.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<(bool result, string error)> RequestDpoToDeleteMyAccount(string medicalRecordNumber)
    {
        var patient =
            await _patientRepository.GetPatientByMedicalRecordNumber(MedicalRecordNumber.Create(medicalRecordNumber));

        if (patient == null)
        {
            return (false, "Patient does not exist.");
        }

        try
        {
            await _emailService.RequestDpoToDeleteMyAccount(patient.ContactInformation.EmailAddress.GetFullEmail(),
                patient.MedicalRecordNumber.Value);
            return (true, "Your Request to delete your personal data was successfully send.");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while sending the request: {ex.Message}");
        }
    }
}




