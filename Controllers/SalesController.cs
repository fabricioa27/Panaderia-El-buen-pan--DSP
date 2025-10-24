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

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
            ViewBag.Ventas = _data.Ventas;
            return View();
        }

        [HttpPost]
        public IActionResult Index(SaleViewModel venta)
        {
            var producto = _data.Productos.FirstOrDefault(p => p.Id == venta.ProductoId);

            if (producto == null || venta.Cantidad <= 0)
            {
                ModelState.AddModelError("", "Datos inválidos o producto no encontrado.");
                ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
                ViewBag.Ventas = _data.Ventas;
                return View(venta);
            }

            if (venta.Cantidad > producto.Stock)
            {
                ModelState.AddModelError("", "No hay suficiente stock para esa venta.");
                ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
                ViewBag.Ventas = _data.Ventas;
                return View(venta);
            }

            // Calcular total y actualizar stock
            venta.PrecioUnitario = producto.Precio;
            venta.Total = producto.Precio * venta.Cantidad;
            venta.Id = _data.Ventas.Any() ? _data.Ventas.Max(v => v.Id) + 1 : 1;
            venta.Fecha = DateTime.Now;
            venta.Producto = producto.Nombre;

            _data.Ventas.Add(venta);
            producto.Stock -= venta.Cantidad;

            ViewBag.Productos = _data.Productos.Where(p => p.Stock > 0).ToList();
            ViewBag.Ventas = _data.Ventas;

            ModelState.Clear();
            return View(new SaleViewModel());
        }
    }
}