using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panaderia_DSP.Models;
using Panaderia_DSP.Services;

namespace Panaderia_DSP.Controllers
{
    [Authorize(Roles = "Vendedor")]
    public class VendedorController : Controller
    {
        private readonly DataService _data;

        public VendedorController(DataService data)
        {
            _data = data;
        }

        [HttpGet]
        public IActionResult Ventas()
        {
            var username = User.Identity?.Name ?? "Desconocido";

            ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
            ViewBag.MisVentas = _data.Ventas
                .Where(v => v.Vendedor == username)
                .ToList();

            return View(new SaleViewModel());
        }

        [HttpPost]
        public IActionResult Ventas(SaleViewModel venta)
        {
            var producto = _data.Productos.FirstOrDefault(p => p.Id == venta.ProductoId);
            var username = User.Identity?.Name ?? "Desconocido";

            if (producto == null || venta.Cantidad <= 0)
            {
                ModelState.AddModelError("", "Datos inválidos.");
                ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
                ViewBag.MisVentas = _data.Ventas.Where(v => v.Vendedor == username).ToList();
                return View(venta);
            }

            if (venta.Cantidad > producto.Stock)
            {
                ModelState.AddModelError("", "Stock insuficiente para esa venta.");
                ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
                ViewBag.MisVentas = _data.Ventas.Where(v => v.Vendedor == username).ToList();
                return View(venta);
            }

            // ✅ Registrar la venta
            venta.Id = _data.Ventas.Any() ? _data.Ventas.Max(v => v.Id) + 1 : 1;
            venta.Fecha = DateTime.Now;
            venta.Producto = producto.Nombre;
            venta.PrecioUnitario = producto.Precio;
            venta.Total = producto.Precio * venta.Cantidad;
            venta.Vendedor = username;

            _data.Ventas.Add(venta);

            // 🔻 Restar stock
            producto.Stock -= venta.Cantidad;

            ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
            ViewBag.MisVentas = _data.Ventas.Where(v => v.Vendedor == username).ToList();

            ModelState.Clear();
            ViewBag.Mensaje = "✅ Venta registrada correctamente.";
            return View(new SaleViewModel());
        }
    }
}
