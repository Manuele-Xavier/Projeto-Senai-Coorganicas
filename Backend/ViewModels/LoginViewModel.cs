using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]        
        [StringLength(255, MinimumLength = 3, ErrorMessage = "A senha deve conter no mínimo 3 caracteres")]
        public string Senha { get; set; }
    }
}