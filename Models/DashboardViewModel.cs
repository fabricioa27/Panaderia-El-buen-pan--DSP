namespace Panaderia_DSP.Models
{
    public class DashboardViewModel
    {
        public int TotalProductos { get; set; }
        public int TotalVentas { get; set; }
        public int ProductosStockBajo { get; set; }
        public decimal TotalVentasHoy { get; set; }
        public List<ProductViewModel> ProductosRecientes { get; set; }
        public List<SaleViewModel> VentasRecientes { get; set; }

        // Métricas adicionales
        public decimal PromedioVenta { get; set; }
        public int VentasHoy { get; set; }
        public string ProductoMasVendido { get; set; }
    }
}