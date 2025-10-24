namespace Panaderia_DSP.Models
{
    public class VendedorViewModel
    {
        public string NombreVendedor { get; set; }
        public List<SaleViewModel> VentasHoy { get; set; }
        public decimal TotalVendidoHoy { get; set; }
        public int TotalVentasHoy { get; set; }
        public List<ProductViewModel> ProductosDisponibles { get; set; }
        public VentaRegistroViewModel NuevaVenta { get; set; }
    }
}