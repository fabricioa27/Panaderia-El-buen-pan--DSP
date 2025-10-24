using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiApi.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string Cliente { get; set; }

        [Required]
        [StringLength(100)]
        public string Vendedor { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [StringLength(300)]
        public string Observaciones { get; set; }

        public Producto Producto { get; set; }
    }
}