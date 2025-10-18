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
        }

        public IActionResult Index()
        {
            ViewData["TotalProductos"] = _data.TotalProductos();
            ViewData["TotalVentas"] = _data.TotalVentas();
            ViewData["StockBajo"] = _data.ProductosConStockBajo();

            return View();
        }
    }
}
