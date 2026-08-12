using App.Onion.Application.Dtos;
using App.Onion.Domain.Interfaces.IMedicalRecordNumberGenerator;
using App.Onion.Domain.Interfaces.PatientRepository;
using dddnet8.Domain.OperationTypes.Names;
using dddnet8.Domain.Patients.DTO;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.Patients.VO.Name;
using dddnet8.Infraestructure.UtilsBootstrapper.SystemUsers;
using SurgicalManagement.Domain.Domain;
using Name = dddnet8.Domain.Patients.V.O.Name;

namespace dddnet8.Infraestructure.UtilsBootstrapper.Patients;

public class PatientUtils
{
    private readonly IPatientRepository _patientRepository;
    private readonly SystemUserUtils _systemUserUtils;


    public PatientUtils(IPatientRepository patientRepository, SystemUserUtils systemUserUtils)
    {
        _patientRepository = patientRepository;
        _systemUserUtils = systemUserUtils;
    }
    
    public async Task InitializePatientsAsync(){
        
        var patients = await _patientRepository.GetAllAsync();

        if (!patients.Any())
        {
            await SavePatient(await CreatePatient("Antonio Maria", "Pereira",
                MedicalRecordNumber.Create("202411000001"),
                new ContactInfoDto("351987654321", "antonioMaria@gmail.com"),
                new EmergencyContactDto("Carlos Silva", "351987654322"),
                DateOfBirth.Create(new DateTime(2003, 5, 20)), Gender.Male));
            
            await SavePatient(await CreatePatient("Alejandro", "Vieira",
                MedicalRecordNumber.Create("202411000002"),
                new ContactInfoDto("351987654311", "alejandroVieira19121@gmail.com"),
                new EmergencyContactDto("Carlos Silva", "351927654322"),
                DateOfBirth.Create(new DateTime(2001, 12, 19)), Gender.Male));
            
            await SavePatient(await CreatePatient("Leo", "Deo",
                MedicalRecordNumber.Create("202411000003"),
                new ContactInfoDto("351987651234", "janedoe1@gmail.com"),
                new EmergencyContactDto("Carlos Silva", "351927654322"),
                DateOfBirth.Create(new DateTime(2001, 12, 20)), Gender.Male));

            await SavePatient(await CreatePatient("Jan", "Doe",
                MedicalRecordNumber.Create("202411000004"),
                new ContactInfoDto("351997651234", "janedoe2@gmail.com"),
                new EmergencyContactDto("Carlos Silva", "351927654322"),
                DateOfBirth.Create(new DateTime(2004, 12, 20)), Gender.Female));

            await SavePatient(await CreatePatient("Jane", "Doe",
                MedicalRecordNumber.Create("202411000005"),
                new ContactInfoDto("351997651534", "janedoe3@gmail.com"),
                new EmergencyContactDto("sadao Silva", "351927654322"),
                DateOfBirth.Create(new DateTime(2004, 12, 20)), Gender.Female));

            await SavePatient(await CreatePatient("Jani", "Doe",
                MedicalRecordNumber.Create("202411000006"),
                new ContactInfoDto("351912913914", "janedoe30@gmail.com"),
                new EmergencyContactDto("dadao Silva", "351927654322"),
                DateOfBirth.Create(new DateTime(2004, 12, 20)), Gender.Female));

            await SavePatient(await CreatePatient("Jala", "Doe",
                MedicalRecordNumber.Create("202411000007"),
                new ContactInfoDto("999888777", "janedoe40@gmail.com"),
                new EmergencyContactDto("Carlos ambrao", "351927654322"),
                DateOfBirth.Create(new DateTime(2004, 12, 20)), Gender.Female)); 
            
            await SavePatient(await CreatePatient("Inexistent", "Doe",
                MedicalRecordNumber.Create("202411000008"),
                new ContactInfoDto("912923001", "inexistent@gmail.com"),
                new EmergencyContactDto("john doe", "351927654322"),
                DateOfBirth.Create(new DateTime(2004, 12, 20)), Gender.Not_Specified));
            
            
            await SavePatient(await CreatePatient("Paulo", "Gandra",
                MedicalRecordNumber.Create("202412000001"),
                new ContactInfoDto("945120332", "pauloGandra@gmail.com"),
                new EmergencyContactDto("john doe", "351927654322"),
                DateOfBirth.Create(new DateTime(1980, 12, 20)), Gender.Male));
            
            await SavePatient(await CreatePatient("John", "Doe",
                MedicalRecordNumber.Create("202412000005"),
                new ContactInfoDto("912923001", "johndoeee09@gmail.com"),
                new EmergencyContactDto("john doe", "351927654322"),
                DateOfBirth.Create(new DateTime(2004, 12, 20)), Gender.Not_Specified));
        }
    }

    private async Task<Patient> CreatePatient(string firstName, string lastName, 
        MedicalRecordNumber medicalRecordNumber, 
        ContactInfoDto contactInfo, 
        EmergencyContactDto emergencyContact, 
        DateOfBirth dateOfBirth,
        Gender gender)
    {
        return new Patient(Name.Create(firstName),
            Name.Create(lastName),
            dateOfBirth,
            gender,
            medicalRecordNumber,
            ContactInfo.Create(contactInfo), 
            EmergencyContact.Create(emergencyContact.EmergencyContactName!, emergencyContact.EmergencyContactPhoneNumber!));
    }


    private async Task SavePatient(Patient patient)
    {
       await _patientRepository.AddPatientAsync(patient);

       if (!patient.MedicalRecordNumber.Value.Equals("20241100008"))
       {
           await _systemUserUtils.CreateAndSaveUser(patient.ContactInformation.EmailAddress.GetFullEmail(), UserRole.Patient, patient.ContactInformation.EmailAddress.GetFullEmail());
       }
    }

    public async Task<Patient?> GetPatient(string medicalRecordNumber)
    {
        return await _patientRepository.GetPatientByMedicalRecordNumber(MedicalRecordNumber.Create(medicalRecordNumber));
    }
}