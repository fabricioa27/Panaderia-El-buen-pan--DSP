using System;

namespace Panaderia_DSP.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
