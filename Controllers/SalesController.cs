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
            ViewBag.Productos = _data.GetProductosConStock();
            ViewBag.Ventas = _data.GetVentas();
            return View(new SaleViewModel());
        }

        [HttpPost]
        public IActionResult Index(SaleViewModel venta)
        {
            venta.Vendedor = User.Identity?.Name ?? "Administrador";

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _data.GetProductosConStock();
                ViewBag.Ventas = _data.GetVentas();
                return View(venta);
            }

            var resultado = _data.RegistrarVenta(venta);

            if (resultado.StartsWith("✅"))
                TempData["Mensaje"] = resultado;
            else
                TempData["Error"] = resultado;

            return RedirectToAction("Index");
        }
    }
}