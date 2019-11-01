using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domains
{
    public partial class Endereco
    {
        [Key]
        [Column("Endereco_id")]
        public int EnderecoId { get; set; }
        [Required]
        [StringLength(255)]
        public string Cidade { get; set; }
        [Required]
        [StringLength(8)]
        public string Cep { get; set; }
        [Required]
        [Column("Endereco")]
        [StringLength(255)]
        public string Endereco1 { get; set; }
        public int? Numero { get; set; }
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("Endereco")]
        public virtual Usuario Usuario { get; set; }
    }
}
