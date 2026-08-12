
using System.Net.Mail;
using System.Text;
using App.Domain.SystemUser;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.Email;


namespace App.EmailSender.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _fromAddress;
        private readonly ISmtpClientWrapper _smtpClientWrapper;
        private readonly string _DPO;

        public EmailService(IConfiguration configuration,
            ISmtpClientWrapper smtpClientWrapper) // Injetando o Wrapper
        {
            _fromAddress = configuration["EmailSettings:FromAddress"];
            _smtpClientWrapper = smtpClientWrapper;
            _DPO = configuration["DataProtectionOfficer"];
        }

        public async Task ActivationAccount(string to, string token)
        {
            if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(token))
                throw new ArgumentException("Email address and token must be provided.");

            var activationLink = $"http://localhost:5000/api/auth/activate-account?token={token}";
            var subject = "Account Activation";
            var body = $@"
                <h1>Welcome!</h1>
                <p>To activate your account, please click the link below:</p>
                <a href='{activationLink}'>Activate Account</a>
                <p>If you did not sign up, you can ignore this email.</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendAdminWarningNotification(IEnumerable<dddnet8.Domain.SystemUsers.SystemUser> admins,
            string username)
        {
            if (admins == null || string.IsNullOrEmpty(username))
                throw new ArgumentException("Admin list and username must be provided.");

            var subject = $"User Account Locked: {username}";
            var body = $@"
                <h1>Account Locked Notification</h1>
                <p>The user <strong>{username}</strong> has been locked out as of <strong>{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}</strong> after exceeding the maximum number of login attempts.</p>
                <p>The user's information has been recorded in the database, and you can view it for further action.</p>
                <p>Please take appropriate measures as needed.</p>";

            foreach (var admin in admins)
            {
                await SendEmailAsync(admin.EmailAddress.ToString(), subject, body);
            }
        }

        public Task ResetPasswordNotification(string userDtoEmailAddress, string token)
        {
            var resetLink = $"http://localhost:5173/reset-password?token={token}";

            string subject = "Redefinição de Senha";
            string body = $@"
            <html>
            <body>
                <h2>Olá!</h2>
                <p>Recebemos um pedido para redefinir a sua senha. Se você não fez esse pedido, pode ignorar este e-mail.</p>
                <p>Para redefinir sua senha, clique no link abaixo:</p>
                <a href='{resetLink}' style='padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px;'>Redefinir Senha</a>
                <p>O link acima é válido por 24 horas. Depois desse período, será necessário solicitar uma nova redefinição de senha.</p>
                <p>Atenciosamente,<br>Equipe de Suporte</p>
            </body>
            </html>";

            // Envie o e-mail usando seu serviço de e-mail preferido.
            return SendEmailAsync(userDtoEmailAddress, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var message = new MailMessage(_fromAddress, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                await _smtpClientWrapper.SendMailAsync(message);
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception($"Error sending email to {to}. Check SMTP configuration.", smtpEx);
            }
        }

        public Task SendAccountActivationConfirmation(SystemUserDto user)
        {
            if (user == null ||
                string.IsNullOrEmpty(user.EmailAddress) ||
                string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Role))
            {
                throw new ArgumentException("User information must be provided, including email, username, and role.");
            }

            string subject = "Welcome to the System!";
            string body = $@"
        <html>
        <body>
            <h2>Welcome, {user.Username}!</h2>
            <p>Your account has been successfully activated.</p>
            <p>You can now access the system using the following credentials:</p>
            <ul>
                <li><strong>Username:</strong> {user.Username}</li>
                <li><strong>Role:</strong> {user.Role}</li>
            </ul>
            <p>We are glad to have you on board! If you have any questions or need assistance, please do not hesitate to reach out.</p>
            <p>Best regards,<br>The Support Team</p>
        </body>
        </html>";

            // Send the email using your preferred email service.
            return SendEmailAsync(user.EmailAddress, subject, body);
        }

        public async Task NotifyClientAboutUpdate(EmailAddress emailAddress)
        {

            string subject = "Profile Update Notification";
            string body = $@"
    <html>
    <body>
        <h2>Greetings,</h2>
        <p>We hope this message finds you well.</p>
        <p>We would like to inform you that there have been recent changes made to your profile. 
        To ensure the accuracy of your information, we kindly ask you to review your profile at your earliest convenience.</p>
        <p>Please log in to your account to check the updated details. If you did not make these changes or if you have any questions or concerns, please do not hesitate to contact us.</p>
        <p>Thank you for your attention to this matter.</p>
        <p>Best regards,<br>The Support Team</p>
    </body>
    </html>";

            await SendEmailAsync(emailAddress.ToString(), subject, body);
        }

        public async Task NotifyAdminsAboutDelete(EmailAddress adminsEmailAddress, List<string> deletedList,
            string whatDeleted)
        {
            if (adminsEmailAddress == null || deletedList == null || !deletedList.Any())
            {
                throw new ArgumentException("Admin email addresses and deleted list must be provided.");
            }

            string subject = "Data Deletion Notification";

            var sb = new StringBuilder();
            sb.AppendLine("<h1>Data Deletion Notification</h1>");
            sb.AppendLine(
                $"<p>The following items have been deleted from the <strong>{whatDeleted}</strong> table:</p>");
            sb.AppendLine("<ul>");

            foreach (var item in deletedList)
            {
                sb.AppendLine($"<li>{item}</li>");
            }

            sb.AppendLine("</ul>");
            sb.AppendLine("<p>For more information, please check the log table.</p>");
            sb.AppendLine("<p>Best regards,<br>Support Team</p>");

            string body = sb.ToString();

            await SendEmailAsync(adminsEmailAddress.ToString(), subject, body);
        }

        public Task RequestDpoToDeleteMyAccount(string patientEmail, string patientMedicalRecordNumber)
        {
            if (string.IsNullOrEmpty(patientEmail) || string.IsNullOrEmpty(patientMedicalRecordNumber))
            {
                throw new ArgumentException("Patient email and medical record number must be provided.");
            }

            if (string.IsNullOrEmpty(_DPO))
            {
                throw new Exception("DPO email is not configured in the application settings.");
            }

            string currentDate = DateTime.Now.ToString("MMMM dd, yyyy"); // Exemplo: "December 05, 2024"

            string subject = "Data Deletion Request from Patient";
            string body = $@"
    <html>
    <body>
        <h2>Data Deletion Request</h2>
        <p>Dear Data Protection Officer,</p>
        <p>We have received a formal request from a patient to delete their personal data as per applicable data protection regulations.</p>
        <p>Below are the details of the patient:</p>
        <ul>
            <li><strong>Email Address:</strong> {patientEmail}</li>
            <li><strong>Medical Record Number:</strong> {patientMedicalRecordNumber}</li>
            <li><strong>Request Date:</strong> {currentDate}</li>
        </ul>
        <p>Please take the necessary steps to ensure the patient's data is deleted from the relevant systems and confirm once the process is complete.</p>
        <p>Thank you for your prompt attention to this matter.</p>
        <p>Best regards,<br>The Support Team</p>
    </body>
    </html>";

            return SendEmailAsync(_DPO, subject, body);
        }
    }
}