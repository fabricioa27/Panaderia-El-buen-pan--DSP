using System.ComponentModel.DataAnnotations;

namespace Panaderia_DSP.Models
{
    public class ProductoCreateViewModel
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 1000, ErrorMessage = "El precio debe estar entre $0.01 y $1000")]
        [Display(Name = "Precio Unitario")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, 1000, ErrorMessage = "El stock debe estar entre 0 y 1000")]
        [Display(Name = "Cantidad en Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Display(Name = "Descripción (opcional)")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; }
    }
}