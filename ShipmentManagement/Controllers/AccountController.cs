using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Account;
using System.Security.Claims;

namespace ShipmentManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Already logged in → redirect to dashboard
            if (_authService.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Index", "Dashboard");

            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Login";
                return View(model);
            }

            // Check credentials
            var success = await _authService.LoginAsync(model, HttpContext.Session);

            if (success)
            {
                // Create Claims (Information about the logged-in user)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Admin") 
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Handling "Remember Me"
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    // If RememberMe is true, cookie lasts 7 days. Otherwise, it's a session cookie.
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
                };

                // Sign in the user to the Cookie Middleware
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),authProperties);

                TempData["Success"] = $"Welcome back!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password. Please try again.");
            ViewData["Title"] = "Login";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _authService.Logout(HttpContext.Session);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult LogoutGet()
        {
            _authService.Logout(HttpContext.Session);
            return RedirectToAction(nameof(Login));
        }
    }
}