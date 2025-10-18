using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panaderia_DSP.Models;
using Panaderia_DSP.Services;

namespace Panaderia_DSP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly DataService _data;

        public ProductsController(DataService data)
        {
            _data = data;
        }

        public IActionResult Index()
        {
            return View(_data.Productos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Id = _data.Productos.Any() ? _data.Productos.Max(p => p.Id) + 1 : 1;
            _data.Productos.Add(model);

            return RedirectToAction("Index", "Dashboard"); // 🔥 vuelve al dashboard
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var producto = _data.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var producto = _data.Productos.FirstOrDefault(p => p.Id == model.Id);
            if (producto == null) return NotFound();

            producto.Nombre = model.Nombre;
            producto.Precio = model.Precio;
            producto.Stock = model.Stock;
            producto.Categoria = model.Categoria;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _data.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
                _data.Productos.Remove(producto);

            return RedirectToAction("Index", "Dashboard"); // 🔥 recarga datos del panel
        }
    }
}
