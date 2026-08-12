using App.Onion.Application.Dtos;
using dddnet8.Domain.Patients.DataModel; // Ajuste o namespace conforme necessário

namespace dddnet8.Domain.Patients.DTO
{

    public static class PatientMapper
    {
        public static PatientDto ToDto(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            return new PatientDto(
                patient.FullName.ToString(),
                patient.Gender.ToString(),
                patient.DateOfBirth.ToString(),
                patient.MedicalRecordNumber.ToString(),
                new ContactInfoDto(patient.ContactInformation.PhoneNumber.ToString(), patient.ContactInformation.EmailAddress.ToString()), // Informações de contato
                new EmergencyContactDto(patient.EmergencyContact.EmergencyContactName.ToString(), patient.EmergencyContact.EmergencyContactPhoneNumber.ToString()) // Contato de emergência
            );

        }
        public static Patient ToDomainModel(PatientDataModel patientDataModel)
        {
            if (patientDataModel == null) throw new ArgumentNullException(nameof(patientDataModel));

            return new Patient(
                patientDataModel.FirstName,
                patientDataModel.LastName,
                patientDataModel.DateOfBirth,
                patientDataModel.Gender,
                patientDataModel.MedicalRecordNumber,
                patientDataModel.ContactInformation,
                patientDataModel.EmergencyContact,
                patientDataModel.DeletionStatus
            );
        }

        // Converte Patient para PatientDataModel
        public static PatientDataModel ToDataModel(Patient patient, Guid? patientId = null)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            return new PatientDataModel(
                patientId,
                patient.FirstName,
                patient.LastName,
                patient.DateOfBirth,
                patient.Gender,
                patient.MedicalRecordNumber,
                patient.ContactInformation,
                patient.EmergencyContact,
                patient.DeletionStatus
            );
        }

    }
}