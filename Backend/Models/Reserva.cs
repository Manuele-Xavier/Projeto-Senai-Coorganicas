using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public partial class Reserva
    {
        [Key]
        [Column("Reserva_id")]
        public int ReservaId { get; set; }
        [Column("Data_Reserva", TypeName = "datetime")]
        public DateTime DataReserva { get; set; }
        public double Quantidade { get; set; }
        [Column("Data_Espera", TypeName = "datetime")]
        public DateTime? DataEspera { get; set; }
        [Required]
        [Column("Status_Reserva")]
        [StringLength(255)]
        public string StatusReserva { get; set; }
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }
        [Column("Oferta_id")]
        public int? OfertaId { get; set; }

        [ForeignKey(nameof(OfertaId))]
        [InverseProperty("Reserva")]
        public virtual Oferta Oferta { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("Reserva")]
        public virtual Usuario Usuario { get; set; }
    }
}
