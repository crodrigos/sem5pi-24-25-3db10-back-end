using NUnit.Framework;
using App.Onion.Domain.V.O.Patient;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.SystemUsers;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;


namespace SurgicalManagement.Tests.Domain
{
    [TestFixture]
    public class PatientTests
    {
        private Patient _patient;

        [SetUp]
        public void SetUp()
        {
            _patient = CreateTestPatient(); // Inicializa um paciente para os testes
        }

        /// <summary>
        /// Verifica se o primeiro nome é inicializado corretamente.
        /// </summary>
        [Test]
        public void FirstName_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.FirstName, Is.EqualTo(Name.Create("John")));
        }

        /// <summary>
        /// Verifica se o sobrenome é inicializado corretamente.
        /// </summary>
        [Test]
        public void LastName_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.LastName, Is.EqualTo(Name.Create("Doe")));
        }

        /// <summary>
        /// Verifica se o nome completo é inicializado corretamente.
        /// </summary>
        [Test]
        public void FullName_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.FullName, Is.EqualTo(Name.Create("John Doe")));
        }

        /// <summary>
        /// Verifica se a data de nascimento é inicializada corretamente.
        /// </summary>
        [Test]
        public void DateOfBirth_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.DateOfBirth, Is.EqualTo(DateOfBirth.Create(new DateTime(1990, 1, 1))));
        }

        /// <summary>
        /// Verifica se o gênero é inicializado corretamente.
        /// </summary>
        [Test]
        public void Gender_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.Gender, Is.EqualTo(Gender.Male));
        }

        /// <summary>
        /// Verifica se o número do registro médico é inicializado corretamente.
        /// </summary>
        [Test]
        public void MedicalRecordNumber_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.MedicalRecordNumber, Is.EqualTo(MedicalRecordNumber.Create("202410123456")));
        }

        /// <summary>
        /// Verifica se as informações de contato são inicializadas corretamente.
        /// </summary>
        [Test]
        public void ContactInformation_ShouldBeInitializedCorrectly()
        {
            var expectedContactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("alejandro@gmail.com"));
            Assert.That(_patient.ContactInformation, Is.EqualTo(expectedContactInfo));
        }

        

        /// <summary>
        /// Verifica se o contato de emergência é inicializado corretamente.
        /// </summary>
        [Test]
        public void EmergencyContact_ShouldBeInitializedCorrectly()
        {
            var expectedEmergencyContact = EmergencyContact.Create("Jane Doe", "987654321");
            Assert.That(_patient.EmergencyContact, Is.EqualTo(expectedEmergencyContact));
        }

        /// <summary>
        /// Verifica se o status de deleção é inicializado corretamente.
        /// </summary>
        [Test]
        public void DeletionStatus_ShouldBeInitializedCorrectly()
        {
            Assert.That(_patient.DeletionStatus.IsToDelete, Is.False);
        }

        /// <summary>
        /// Testa se a atualização do primeiro nome altera o nome e o nome completo.
        /// </summary>
        [Test]
        public void UpdateFirstName_ShouldChangeFirstNameAndFullName()
        {
            var newFirstName = Name.Create("Jane");
            _patient.UpdatePatient(new PatientCriteria { FirstName = newFirstName.Value });

            Assert.Multiple(() =>
            {
                Assert.That(_patient.FirstName, Is.EqualTo(newFirstName));
                Assert.That(_patient.FullName, Is.EqualTo(Name.Create("Jane Doe")));
            });
        }

        /// <summary>
        /// Testa se a atualização da data de nascimento altera corretamente a data de nascimento do paciente.
        /// </summary>
        [Test]
        public void UpdateDateOfBirth_ShouldChangeDateOfBirth()
        {
            var newDateOfBirth = new DateTime(1985, 5, 5);
            _patient.UpdatePatient(new PatientCriteria { DateOfBirth = newDateOfBirth });

            Assert.That(_patient.DateOfBirth, Is.EqualTo(DateOfBirth.Create(newDateOfBirth)));
        }

        /// <summary>
        /// Testa se a atualização do gênero altera corretamente o gênero do paciente quando um gênero válido é fornecido.
        /// </summary>
        [Test]
        public void UpdateGender_ShouldChangeGender_WhenValidGenderProvided()
        {
            var newGender = "Female";
            _patient.UpdatePatient(new PatientCriteria { Gender = newGender });

            Assert.That(_patient.Gender, Is.EqualTo(Gender.Female));
        }

        /// <summary>
        /// Testa se uma exceção é lançada ao tentar atualizar o gênero com um valor inválido.
        /// </summary>
        [Test]
        public void UpdateGender_ShouldThrowArgumentException_WhenInvalidGenderProvided()
        {
            var invalidGender = "InvalidGender";
            var exception = Assert.Throws<ArgumentException>(() => _patient.UpdatePatient(new PatientCriteria { Gender = invalidGender }));

            Assert.That(exception.Message, Is.EqualTo($"'{invalidGender}' não é um valor válido para o gênero."));
        }

        /// <summary>
        /// Testa se o método MarkForDeletion altera o status de deleção corretamente.
        /// </summary>
        [Test]
        public void MarkForDeletion_ShouldSetDeletionStatus()
        {
            _patient.MarkForDeletion();
            Assert.That(_patient.DeletionStatus.IsToDelete, Is.True);
        }

        /// <summary>
        /// Testa se o método CanDelete retorna falso quando o paciente está marcado para deleção.
        /// </summary>
        [Test]
        public void CanDelete_ShouldReturnFalse_WhenPatientIsMarkedForDeletion()
        {
            _patient.MarkForDeletion();
            Assert.That(_patient.CanDelete(), Is.False);
        }

        private Patient CreateTestPatient()
        {
            var firstName = Name.Create("John");
            var lastName = Name.Create("Doe");
            var dob = DateOfBirth.Create(new DateTime(1990, 1, 1));
            var gender = Gender.Male;
            var medicalRecordNumber = MedicalRecordNumber.Create("202410123456");
            var contactInfo = new ContactInfo(PhoneNumber.Create("923211160"), EmailAddress.Create("alejandro@gmail.com"));
            var emergencyContact = EmergencyContact.Create("Jane Doe", "987654321");

            return new Patient(firstName, lastName, dob, gender, medicalRecordNumber, contactInfo, emergencyContact);
        }
    }
}
