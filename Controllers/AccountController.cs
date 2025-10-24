using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Panaderia_DSP.Controllers
{
    public class AccountController : Controller
    {
        // 🌐 GET: Página de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay sesión activa, redirigir según el rol
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Dashboard");
                else if (User.IsInRole("Vendedor"))
                    return RedirectToAction("Ventas", "Vendedor");
            }

            return View();
        }

        // ✅ POST: Iniciar sesión
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Debe ingresar usuario y contraseña.";
                return View();
            }

            // 🧠 Validación básica de usuarios (solo para el desafío)
            if (username == "admin" && password == "1234")
            {
                // 👑 Rol administrador
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }
            else if (username == "vendedor" && password == "1234")
            {
                // 🧍 Rol vendedor
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Vendedor")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Ventas", "Vendedor");
            }
            else
            {
                // ❌ Credenciales incorrectas
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        // 🚪 GET: Cerrar sesión
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // 🚫 GET: Acceso denegado (para rutas restringidas)
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
