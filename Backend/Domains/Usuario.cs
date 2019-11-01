using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domains
{
    public partial class Usuario
    {
        public Usuario()
        {
            Endereco = new HashSet<Endereco>();
            Oferta = new HashSet<Oferta>();
            Receita = new HashSet<Receita>();
            Reserva = new HashSet<Reserva>();
            Telefone = new HashSet<Telefone>();
        }

        [Key]
        [Column("Usuario_id")]
        public int UsuarioId { get; set; }
        [Required]
        [StringLength(255)]
        public string Nome { get; set; }
        [Required]
        [Column("CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }
        [Required]
        [StringLength(255)]
        public string Senha { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Column("Imagem_Usuario")]
        [StringLength(255)]
        public string ImagemUsuario { get; set; }
        [Column("Tipo_usuario_id")]
        public int? TipoUsuarioId { get; set; }

        [ForeignKey(nameof(TipoUsuarioId))]
        [InverseProperty("Usuario")]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Endereco> Endereco { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Oferta> Oferta { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Receita> Receita { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Reserva> Reserva { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Telefone> Telefone { get; set; }
    }
}
