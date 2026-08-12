using System.Net.Mail;
using App.Domain.SystemUser;
using App.EmailSender.Service;
using dddnet8.Domain.Patients.V.O;
using dddnet8.Domain.SystemUsers;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SurgicalManagement.Domain.Domain;
using dddnet8.Domain.Patients.VO.Name;


namespace dddnet8.Tests.Infraestructure.Email
{
    /// <summary>
    /// Unit tests for the EmailService class.
    /// </summary>
    public class EmailServiceTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ISmtpClientWrapper> _mockSmtpClientWrapper;
        private EmailService _emailService;

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["EmailSettings:FromAddress"]).Returns("test@example.com");
            _mockConfiguration.Setup(config => config["EmailSettings:FromPassword"]).Returns("password");
            _mockConfiguration.Setup(config => config["EmailSettings:SmtpHost"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(config => config["EmailSettings:SmtpPort"]).Returns("587");

            _mockSmtpClientWrapper = new Mock<ISmtpClientWrapper>();

            _emailService = new EmailService(_mockConfiguration.Object, _mockSmtpClientWrapper.Object);
        }

        /// <summary>
        /// Test the account activation email sending functionality.
        /// </summary>
        [Test]
        public async Task ActivationAccount_ShouldSendEmail_WhenValidInput()
        {
            // Arrange
            var to = "alejandrovieira1912@gmail.com";
            var token = "activation_token";

            // Act
            await _emailService.ActivationAccount(to, token);

            // Assert
            _mockSmtpClientWrapper.Verify(smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                msg.From.Address == "test@example.com" && // Verifica o endereço de envio
                msg.To[0].Address == to && // Verifica o destinatário
                msg.Subject == "Account Activation" && // Verifica o assunto
                msg.Body.Contains(token))));
        }

        /// <summary>
        /// Test sending an admin warning notification email.
        /// </summary>
        [Test]
        public async Task SendAdminWarningNotification_ShouldSendEmails_WhenValidInput()
        {
            // Arrange
            var admins = new List<SystemUser>
            {
                CreateSystemUser1(),
                CreateSystemUser2()
            };

            var username = "locked_user";

            // Act
            await _emailService.SendAdminWarningNotification(admins, username);

            // Assert
            // Aqui você deve verificar se os e-mails foram enviados para todos os administradores.
            _mockSmtpClientWrapper.Verify(smtp => smtp.SendMailAsync(It.IsAny<MailMessage>()));
        }

        /// <summary>
        /// Test the password reset notification email sending functionality.
        /// </summary>
        [Test]
        public async Task ResetPasswordNotification_ShouldSendEmail_WhenValidInput()
        {
            // Arrange
            var userDtoEmailAddress = "user@example.com";
            var token = "reset_token";

            // Act
            await _emailService.ResetPasswordNotification(userDtoEmailAddress, token);

            // Assert
            _mockSmtpClientWrapper.Verify(smtp => smtp.SendMailAsync(It.IsAny<MailMessage>()));
        }

        /// <summary>
        /// Test the account activation confirmation email sending functionality.
        /// </summary>
        [Test]
       public async Task SendAccountActivationConfirmation_ShouldSendEmail_WhenValidInput() {
            // Arrange
            var user = new SystemUserDto(
                "user@example.com",
                "alejandrovieira1912@gmail.com",
                "Patient");

            // Act
          await _emailService.SendAccountActivationConfirmation(user);

            // Assert
            _mockSmtpClientWrapper.Verify(smtp => smtp.SendMailAsync(It.IsAny<MailMessage>()));
        }

        /// <summary>
        /// Test the patient update notification email sending functionality.
        /// </summary>
        [Test]
        public void NotificatePatientAboutUpdate_ShouldThrowNotImplementedException()
        {
            // Arrange
            var name = Name.Create("Teste Example");
            var emailAddress = EmailAddress.Create("alejandrovieira1912@gmail.com");

            // Act & Assert
            
        }
        
        /// <summary>
        /// Test the notification email sending functionality for deleted items.
        /// </summary>
        [Test]
        public async Task NotifyAdminsAboutDelete_ShouldSendEmails_WhenValidInput()
        {
            // Arrange

            var admin = EmailAddress.Create("admin1@example.com");
               
          
            var deletedItems = new List<string>
            {
                "Record 1",
                "Record 2",
                "Record 3"
            };
            var whatDeleted = "Patient Records";

            // Act
            await _emailService.NotifyAdminsAboutDelete(admin, deletedItems, whatDeleted);

            // Assert
            
                _mockSmtpClientWrapper.Verify(smtp => smtp.SendMailAsync(It.Is<MailMessage>(msg =>
                        msg.From.Address == "test@example.com" &&
                        msg.To[0].Address == admin.ToString() && 
                        msg.Subject == "Data Deletion Notification" && // Verifica o assunto
                        msg.Body.Contains("Record 1") && 
                        msg.Body.Contains("Record 2") && 
                        msg.Body.Contains("Record 3") && 
                        msg.Body.Contains(whatDeleted) 
                )), Times.Once);
        }

        
        
        
        
        private SystemUser CreateSystemUser1()
        {
            return new SystemUser(EmailAddress.Create("A123@trelloHospital.com"), EmailAddress.Create("undercoverspace18@gmail.com"), UserRole.Admin, "ForT@stPurpose2024");
        }
        
        private SystemUser CreateSystemUser2()
        {
            return new SystemUser(EmailAddress.Create("A245@trelloHospital.com"), EmailAddress.Create("alejandrovieira1912@gmail.com"), UserRole.Admin, "ForT@stPurpose2024");
        }
    }
}
