using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmerMarket.Models;

namespace FarmerMarket.Controllers
{
    public class AdminController : Controller
    {
        private readonly FarmerMarketContext _context;

        public AdminController(FarmerMarketContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        public IActionResult Dashboard()
        {
            // Check if user is admin
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied. Admin only.";
                return RedirectToAction("Index", "Home");
            }

            // Get statistics
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalFarmers = _context.Users.Count(u => u.UserType == "Farmer");
            ViewBag.TotalBuyers = _context.Users.Count(u => u.UserType == "Buyer");
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalInquiries = _context.Inquiries.Count();
            ViewBag.PendingInquiries = _context.Inquiries.Count(i => i.Status == "Pending");

            return View();
        }

        // View All Users
        public IActionResult Users()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _context.Users.OrderByDescending(u => u.CreatedAt).ToList();
            return View(users);
        }

        // View All Products
        public IActionResult Products()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var products = _context.Products
                .Include(p => p.Farmer)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
            return View(products);
        }

        // View All Inquiries
        public IActionResult Inquiries()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var inquiries = _context.Inquiries
                .Include(i => i.Product)
                .Include(i => i.Buyer)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();
            return View(inquiries);
        }

        // Delete User
        public IActionResult DeleteUser(int id)
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.Find(id);
            if (user != null && user.UserType != "Admin")
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "User deleted successfully!";
            }

            return RedirectToAction("Users");
        }

        // Delete Product
        public IActionResult DeleteProduct(int id)
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }

            return RedirectToAction("Products");
        }
    }
}
