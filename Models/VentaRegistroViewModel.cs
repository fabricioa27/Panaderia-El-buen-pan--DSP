using System.ComponentModel.DataAnnotations;

namespace Panaderia_DSP.Models
{
    public class VentaRegistroViewModel
    {
        [Required(ErrorMessage = "El cliente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre del cliente no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Cliente")]
        public string Cliente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Observaciones (opcional)")]
        [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder 300 caracteres")]
        public string Observaciones { get; set; }

        // Propiedades para mostrar en la vista
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Stock Disponible")]
        public int StockDisponible { get; set; }
    }
}