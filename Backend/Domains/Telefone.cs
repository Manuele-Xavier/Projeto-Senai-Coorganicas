using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Domains
{
    public partial class Telefone
    {
        [Key]
        [Column("Telefone_id")]
        public int TelefoneId { get; set; }
        [Column("Telefone")]
        [StringLength(255)]
        public string Telefone1 { get; set; }
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("Telefone")]
        public virtual Usuario Usuario { get; set; }
    }
}
