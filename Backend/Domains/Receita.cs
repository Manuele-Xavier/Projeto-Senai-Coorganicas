using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domains
{
    public partial class Receita
    {
        [Key]
        [Column("Receita_id")]
        public int ReceitaId { get; set; }
        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Conteudo { get; set; }
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }
        [Column("Imagem_Receita")]
        [StringLength(255)]
        public string ImagemReceita { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("Receita")]
        public virtual Usuario Usuario { get; set; }
    }
}
