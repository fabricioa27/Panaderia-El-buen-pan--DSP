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
            var username = User.Identity?.Name ?? "Vendedor";

            ViewBag.Productos = _data.GetProductosConStock();
            ViewBag.MisVentas = _data.GetVentasPorVendedor(username);

            return View(new SaleViewModel());
        }

        [HttpPost]
        public IActionResult Ventas(SaleViewModel venta)
        {
            var username = User.Identity?.Name ?? "Vendedor";
            venta.Vendedor = username;

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _data.GetProductosConStock();
                ViewBag.MisVentas = _data.GetVentasPorVendedor(username);
                return View(venta);
            }

            var resultado = _data.RegistrarVenta(venta);
            ViewBag.Mensaje = resultado;

            // Recargar datos
            ViewBag.Productos = _data.GetProductosConStock();
            ViewBag.MisVentas = _data.GetVentasPorVendedor(username);

            return View(new SaleViewModel());
        }
    }
}