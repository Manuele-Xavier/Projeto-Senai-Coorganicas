using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public partial class Oferta
    {
        public Oferta()
        {
            Reserva = new HashSet<Reserva>();
        }

        [Key]
        [Column("Oferta_id")]
        public int OfertaId { get; set; }
        [Column(TypeName = "money")]
        public decimal Preco { get; set; }
        
        // [Required]
        [StringLength(255)]
        public string Cidade { get; set; }
        [Column(TypeName = "date")]
        public DateTime Validade { get; set; }
        public double Quantidade { get; set; }
        // [Required]
        [StringLength(255)]
        public string Regiao { get; set; }
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }
        [Column("Produto_id")]
        public int? ProdutoId { get; set; }

        [ForeignKey(nameof(ProdutoId))]
        [InverseProperty("Oferta")]
        public virtual Produto Produto { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("Oferta")]
        public virtual Usuario Usuario { get; set; }
        [InverseProperty("Oferta")]
        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}
