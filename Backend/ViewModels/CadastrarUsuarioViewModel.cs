using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class CadastrarUsuarioViewModel
    {   
        [Required(ErrorMessage = "Nome do usuário obrigatório")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Cnpj é obrigatório")]
        [StringLength(255, MinimumLength = 14, ErrorMessage = "Cnpj Inválido")]
        [MaxLength(14)]
        public string Cnpj { get; set; }
        
        [Required(ErrorMessage = "Senha é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "A senha deve conter no mínimo 3 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail com formato inválido")]
        public string Email { get; set; }
        public string ImagemUsuario { get; set; }

        [Required(ErrorMessage = "O tipo do usuario é obrigatório")]    
        [RegularExpression("([0-3])", ErrorMessage = "Tipo de usuário inválido selecione comunidade ou agricultor")]
        public int TipoUsuarioId { get; set; }
        
        [DataType(DataType.Password)]       
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public string ConfirmarSenha { get; set; }
        
        [Required(ErrorMessage = "Telefone é obrigatório")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "Informe o telefone com o DDD")]
        [MaxLength(11)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatório")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Informe a sua cidade")]        
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Cep é obrigatório")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Cep incorreto")]   
        [MaxLength(8)]     
        public string Cep { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Informe o endereço")]     
        public string Endereco { get; set; }

        [Range(1, 9999, ErrorMessage = "Número da residência inválido")]
        public int Numero { get; set; }
    }
}