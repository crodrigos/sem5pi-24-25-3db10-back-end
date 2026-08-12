namespace App.SystemUser.Domain.DTO
{
    public class PasswordDto
    {
        // Propriedade para a nova senha
        public string Password { get; set; }

        // Propriedade para a confirmação da nova senha
        public string Confirmation { get; set; }
    }
}