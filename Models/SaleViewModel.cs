using System.ComponentModel.DataAnnotations;

namespace Panaderia_DSP.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de venta")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Cliente")]
        public string Cliente { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        public string Producto { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Ingrese una cantidad válida")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Vendedor")]
        public string Vendedor { get; set; }
    }
}