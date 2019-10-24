using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public partial class Produto
    {
        public Produto()
        {
            Oferta = new HashSet<Oferta>();
        }

        [Key]
        [Column("Produto_id")]
        public int ProdutoId { get; set; }
        [Required]
        [StringLength(255)]
        public string Nome { get; set; }
        [Column("Imagem_Produto")]
        public byte[] ImagemProduto { get; set; }
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; }

        [InverseProperty("Produto")]
        public virtual ICollection<Oferta> Oferta { get; set; }
    }
}
