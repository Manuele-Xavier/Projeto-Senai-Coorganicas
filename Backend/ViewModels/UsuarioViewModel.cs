using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class UsuarioViewModel
    {                   
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [StringLength(255, MinimumLength = 14, ErrorMessage = "Cnpj Inválido")]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "A senha deve conter no mínimo 3 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
        public string Email { get; set; }
        public string ImagemUsuario { get; set; }
       
        [RegularExpression("([0-3])", ErrorMessage = "Tipo de usuário inválido consulte o administrador do sistema!")]
        public int TipoUsuarioId { get; set; }

        [DataType(DataType.Password)]       
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public string ConfirmarSenha { get; set; }

        [StringLength(255, MinimumLength = 10, ErrorMessage = "Informe o telefone com o DDD")]
        [MaxLength(11)]
        public string Telefone { get; set; }

        [StringLength(255, MinimumLength = 2, ErrorMessage = "Informe a sua cidade")]        
        public string Cidade { get; set; }

        [StringLength(255, MinimumLength = 8, ErrorMessage = "Cep incorreto")]   
        [MaxLength(8)]     
        public string Cep { get; set; }

        [StringLength(255, MinimumLength = 5, ErrorMessage = "Informe o endereço")]     
        public string Endereco { get; set; }

        
        [RegularExpression("([0-9]+)", ErrorMessage = "Informe o número da residencia")]
        public int Numero { get; set; }
    }
}