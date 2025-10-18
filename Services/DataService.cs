using Panaderia_DSP.Models;

namespace Panaderia_DSP.Services
{
    public class DataService
    {
        // 🔹 Listas compartidas simulando la base de datos
        public List<ProductViewModel> Productos { get; set; } = new();
        public List<SaleViewModel> Ventas { get; set; } = new();

        public DataService()
        {
            // Datos iniciales opcionales
            Productos = new List<ProductViewModel>
            {
                new ProductViewModel { Id = 1, Nombre = "Pan Francés", Precio = 0.25m, Stock = 10, Categoria = "Panadería" },
                new ProductViewModel { Id = 2, Nombre = "Croissant", Precio = 0.75m, Stock = 3, Categoria = "Pastelería" },
                new ProductViewModel { Id = 3, Nombre = "Concha", Precio = 0.50m, Stock = 0, Categoria = "Dulce" },
            };

            Ventas = new List<SaleViewModel>();
        }

        // 🔸 Métodos de ayuda
        public int TotalProductos() => Productos.Count;
        public int TotalVentas() => Ventas.Count;
        public int ProductosConStockBajo() => Productos.Count(p => p.Stock <= 5);
    }
}
