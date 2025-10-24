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
            var productos = _data.GetProductos();
            return View(productos);
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

            if (_data.AgregarProducto(model))
                TempData["Mensaje"] = "✅ Producto creado exitosamente";
            else
                TempData["Error"] = "❌ Error al crear el producto";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var producto = _data.GetProductoById(id);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado";
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_data.ActualizarProducto(model))
                TempData["Mensaje"] = "✅ Producto actualizado exitosamente";
            else
                TempData["Error"] = "❌ Error al actualizar el producto";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_data.EliminarProducto(id))
                TempData["Mensaje"] = "✅ Producto eliminado exitosamente";
            else
                TempData["Error"] = "❌ Error al eliminar el producto";

            return RedirectToAction("Index");
        }
    }
}