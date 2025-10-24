using System.ComponentModel.DataAnnotations;

namespace Panaderia_DSP.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar el precio")]
        [Range(0.01, 9999.99, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el stock")]
        [Range(0, 1000, ErrorMessage = "El stock debe estar entre 0 y 1000")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe ingresar la categoría")]
        public string Categoria { get; set; }

        // ✅ AGREGAR ESTA PROPIEDAD QUE FALTABA
        public int CategoriaId { get; set; }
    }
}