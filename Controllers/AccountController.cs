using Microsoft.AspNetCore.Mvc;
using TasksManager.ViewModels;
using TasksManager.Services;
using Microsoft.AspNetCore.Http;

namespace TasksManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var token = await _usersService.Authenticate(model);
                if (!string.IsNullOrEmpty(token))
                {
                    // Store token in session
                    HttpContext.Session.SetString("JWTToken", token);

                    // Redirect to returnUrl if provided, otherwise to home
                    return Redirect(returnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersService.Register(model);
                if (result)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Registration failed. Please try again.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}