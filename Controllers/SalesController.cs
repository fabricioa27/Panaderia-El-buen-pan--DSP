using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panaderia_DSP.Models;
using Panaderia_DSP.Services;

namespace Panaderia_DSP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesController : Controller
    {
        private readonly DataService _data;

        public SalesController(DataService data)
        {
            _data = data;
        }

        public IActionResult Index()
        {
            return View(_data.Ventas);
        }

        [HttpPost]
        public IActionResult CrearVenta(int productoId, int cantidad)
        {
            var producto = _data.Productos.FirstOrDefault(p => p.Id == productoId);
            if (producto == null || producto.Stock < cantidad)
                return RedirectToAction("Index");

            var total = producto.Precio * cantidad;

            _data.Ventas.Add(new SaleViewModel
            {
                Id = _data.Ventas.Any() ? _data.Ventas.Max(v => v.Id) + 1 : 1,
                Producto = producto.Nombre,
                Cantidad = cantidad,
                Total = total
            });

            producto.Stock -= cantidad;

            return RedirectToAction("Index", "Dashboard"); // 🔥 recarga estadísticas
        }
    }
}
