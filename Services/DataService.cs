using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using MiApi.Models;
using Panaderia_DSP.Models;

namespace Panaderia_DSP.Services
{
    public class DataService
    {
        private readonly AppDbContext _context;

        public DataService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ INICIALIZAR CATEGORÍAS
        public void InicializarCategorias()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.AddRange(
                    new Categoria { Nombre = "Pan Dulce" },
                    new Categoria { Nombre = "Pan Salado" },
                    new Categoria { Nombre = "Bebidas" },
                    new Categoria { Nombre = "Repostería" },
                    new Categoria { Nombre = "Galletas" },
                    new Categoria { Nombre = "Otros" }
                );
                _context.SaveChanges();
            }
        }

        // ✅ PRODUCTOS - FUNCIONANDO
        public List<ProductViewModel> GetProductos()
        {
            return _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    CategoriaId = p.CategoriaId
                })
                .ToList();
        }

        // ✅ OBTENER PRODUCTO POR ID - FUNCIONANDO
        public ProductViewModel GetProductoById(int id)
        {
            var producto = _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefault(p => p.Id == id && p.Activo);

            if (producto == null) return null;

            return new ProductViewModel
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Categoria = producto.Categoria.Nombre,
                CategoriaId = producto.CategoriaId
            };
        }

        // ✅ AGREGAR PRODUCTO - SIMPLE Y FUNCIONANDO
        public bool AgregarProducto(ProductViewModel model)
        {
            try
            {
                // Buscar categoría por nombre (como lo esperan las vistas)
                var categoria = _context.Categorias.FirstOrDefault(c => c.Nombre == model.Categoria);
                if (categoria == null)
                {
                    // Si no existe, usar la primera categoría disponible
                    categoria = _context.Categorias.FirstOrDefault();
                    if (categoria == null) return false;
                }

                var producto = new Producto
                {
                    Nombre = model.Nombre,
                    Precio = model.Precio,
                    Stock = model.Stock,
                    CategoriaId = categoria.Id,
                    FechaRegistro = DateTime.Now,
                    Activo = true
                };

                _context.Productos.Add(producto);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ✅ ACTUALIZAR PRODUCTO - SIMPLE Y FUNCIONANDO
        public bool ActualizarProducto(ProductViewModel model)
        {
            try
            {
                var producto = _context.Productos.Find(model.Id);
                if (producto == null) return false;

                // Buscar categoría
                var categoria = _context.Categorias.FirstOrDefault(c => c.Nombre == model.Categoria);
                if (categoria == null) return false;

                producto.Nombre = model.Nombre;
                producto.Precio = model.Precio;
                producto.Stock = model.Stock;
                producto.CategoriaId = categoria.Id;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ✅ ELIMINAR PRODUCTO - FUNCIONANDO
        public bool EliminarProducto(int id)
        {
            try
            {
                var producto = _context.Productos.Find(id);
                if (producto == null) return false;

                producto.Activo = false;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ✅ VENTAS - FUNCIONANDO
        public List<SaleViewModel> GetVentas()
        {
            return _context.Ventas
                .Include(v => v.Producto)
                .OrderByDescending(v => v.FechaVenta)
                .Select(v => new SaleViewModel
                {
                    Id = v.Id,
                    Fecha = v.FechaVenta,
                    ProductoId = v.ProductoId,
                    Producto = v.Producto.Nombre,
                    Cantidad = v.Cantidad,
                    PrecioUnitario = v.Producto.Precio,
                    Total = v.Total,
                    Cliente = v.Cliente,
                    Vendedor = v.Vendedor
                })
                .ToList();
        }

        // ✅ VENTAS POR VENDEDOR - FUNCIONANDO
        public List<SaleViewModel> GetVentasPorVendedor(string vendedor)
        {
            return _context.Ventas
                .Include(v => v.Producto)
                .Where(v => v.Vendedor == vendedor)
                .OrderByDescending(v => v.FechaVenta)
                .Select(v => new SaleViewModel
                {
                    Id = v.Id,
                    Fecha = v.FechaVenta,
                    ProductoId = v.ProductoId,
                    Producto = v.Producto.Nombre,
                    Cantidad = v.Cantidad,
                    PrecioUnitario = v.Producto.Precio,
                    Total = v.Total,
                    Cliente = v.Cliente,
                    Vendedor = v.Vendedor
                })
                .ToList();
        }

        // ✅ REGISTRAR VENTA - FUNCIONANDO
        public string RegistrarVenta(SaleViewModel ventaVM)
        {
            try
            {
                var producto = _context.Productos.Find(ventaVM.ProductoId);
                if (producto == null)
                    return "Error: Producto no encontrado";

                if (ventaVM.Cantidad <= 0)
                    return "Error: Cantidad debe ser mayor a 0";

                if (ventaVM.Cantidad > producto.Stock)
                    return $"Error: Stock insuficiente. Stock disponible: {producto.Stock}";

                // Calcular total
                var total = producto.Precio * ventaVM.Cantidad;

                // Actualizar stock
                producto.Stock -= ventaVM.Cantidad;

                // Crear venta
                var venta = new Venta
                {
                    ProductoId = ventaVM.ProductoId,
                    Cantidad = ventaVM.Cantidad,
                    Cliente = ventaVM.Cliente,
                    Vendedor = ventaVM.Vendedor,
                    Total = total,
                    FechaVenta = DateTime.Now
                };

                _context.Ventas.Add(venta);
                _context.SaveChanges();

                return $"✅ Venta registrada: {ventaVM.Cantidad} x {producto.Nombre} - Total: ${total}";
            }
            catch (Exception ex)
            {
                return $"Error al registrar venta: {ex.Message}";
            }
        }

        // ✅ PRODUCTOS CON STOCK DISPONIBLE - FUNCIONANDO
        public List<ProductViewModel> GetProductosConStock()
        {
            return _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Stock > 0 && p.Activo)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria.Nombre,
                    CategoriaId = p.CategoriaId
                })
                .ToList();
        }

        // ✅ MÉTODOS DEL DASHBOARD - FUNCIONANDO
        public int TotalProductos() => _context.Productos.Count(p => p.Activo);
        public int TotalVentas() => _context.Ventas.Count();
        public int ProductosConStockBajo() => _context.Productos.Count(p => p.Stock <= 5 && p.Activo);

        // ✅ OBTENER CATEGORÍAS - FUNCIONANDO
        public List<Categoria> GetCategorias() => _context.Categorias.ToList();
    }
}