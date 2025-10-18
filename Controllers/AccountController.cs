using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Panaderia_DSP.Models;

namespace Panaderia_DSP.Controllers
{
    public class AccountController : Controller
    {
        // ✅ Vista de login (GET)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirige a la página de inicio según el rol
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Inicio");
            }

            return View();
        }

        // ✅ Procesar inicio de sesión (POST)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // 🔐 Usuarios fijos por ahora
            var usuarios = new List<(string Email, string Password, string Rol)>
            {
                ("admin@panaderia.com", "1234", "Admin"),
                ("vendedor@panaderia.com", "1234", "Vendedor")
            };

            // Buscar usuario
            var usuario = usuarios.FirstOrDefault(u =>
                u.Email.Equals(vm.Email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == vm.Password);

            if (usuario.Equals(default((string, string, string))))
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos");
                return View(vm);
            }

            // Crear las Claims (identidad del usuario)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Iniciar sesión
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = vm.RememberMe });

            // ✅ Redirige al inicio general (cada rol verá su panel ahí)
            return RedirectToAction("Inicio");
        }

        // ✅ Página de inicio después de loguearse
        [Authorize]
        public IActionResult Inicio()
        {
            return View();
        }

        // ✅ Cerrar sesión
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
