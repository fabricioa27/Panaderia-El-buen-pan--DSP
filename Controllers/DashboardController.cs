using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panaderia_DSP.Services;

namespace Panaderia_DSP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataService _data;

        public DashboardController(DataService data)
        {
            _data = data;
            _data.InicializarCategorias(); // ✅ Nombre correcto
        }

        public IActionResult Index()
        {
            ViewData["TotalProductos"] = _data.TotalProductos(); // ✅ Nombre correcto
            ViewData["TotalVentas"] = _data.TotalVentas(); // ✅ Nombre correcto
            ViewData["StockBajo"] = _data.ProductosConStockBajo(); // ✅ Nombre correcto

            return View();
        }
    }
}