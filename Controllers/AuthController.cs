using Microsoft.AspNetCore.Mvc;
using FarmerMarket.Models;

namespace FarmerMarket.Controllers
{
    public class AuthController : Controller
    {
        private readonly FarmerMarketContext _context;

        public AuthController(FarmerMarketContext context)
        {
            _context = context;
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register (Practical 14 - Validation)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(user);
                }

                user.CreatedAt = DateTime.Now;
                _context.Users.Add(user);
                _context.SaveChanges();

                // Practical 13: TempData for success message
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login (Practical 9 - Session)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Practical 9: Store user info in session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserType", user.UserType);

                // Practical 9: Set cookie for "Remember Me"
                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("UserEmail", user.Email, option);

                // Redirect based on user type
                if (user.UserType == "Farmer")
                {
                    return RedirectToAction("Dashboard", "Farmer");
                }
                else if (user.UserType == "Buyer")
                {
                    return RedirectToAction("Browse", "Buyer");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }

            TempData["ErrorMessage"] = "Invalid email or password";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UserEmail");
            TempData["SuccessMessage"] = "Logged out successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
