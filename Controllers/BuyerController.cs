using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FarmerMarket.Models;
using FarmerMarket.Services;

namespace FarmerMarket.Controllers
{
    public class BuyerController : Controller
    {
        private readonly FarmerMarketContext _context;
        private readonly IMemoryCache _cache;

        public BuyerController(FarmerMarketContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Browse Products (Practical 4, 10)
        public IActionResult Browse(string? category, string? search, string? sortBy)
        {
            // Practical 10: Response Caching
            string cacheKey = $"AllProducts_{category}_{search}_{sortBy}";
            if (!_cache.TryGetValue(cacheKey, out List<Product> products))
            {
                products = _context.Products
                    .Include(p => p.Farmer)
                    .AsQueryable()
                    .ToList();

                // Practical 4: Query string filtering
                if (!string.IsNullOrEmpty(category))
                {
                    products = products.Where(p => p.Category == category).ToList();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => 
                        p.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Sorting
                if (sortBy == "price_low")
                {
                    products = products.OrderBy(p => p.Price).ToList();
                }
                else if (sortBy == "price_high")
                {
                    products = products.OrderByDescending(p => p.Price).ToList();
                }
                else
                {
                    products = products.OrderByDescending(p => p.CreatedAt).ToList();
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, products, cacheOptions);
            }

            // Practical 13: ViewBag for filter values
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sortBy;

            return View(products);
        }

        // Product Details (Practical 4)
        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // Practical 7: Using extension methods
            ViewBag.FormattedPrice = product.GetFormattedPrice();
            ViewBag.TotalValue = product.CalculateTotalValue();
            ViewBag.IsLowStock = product.IsLowStock();

            return View(product);
        }

        // Send Inquiry (Practical 14, 15)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendInquiry(Inquiry inquiry)
        {
            var buyerId = HttpContext.Session.GetInt32("UserId");
            if (buyerId == null)
            {
                TempData["ErrorMessage"] = "Please login to send inquiry";
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                inquiry.BuyerId = buyerId.Value;
                inquiry.CreatedAt = DateTime.Now;
                inquiry.Status = "Pending";

                _context.Inquiries.Add(inquiry);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Inquiry sent successfully!";
                return RedirectToAction("Browse");
            }

            return RedirectToAction("ProductDetails", new { id = inquiry.ProductId });
        }

        // My Inquiries
        public IActionResult MyInquiries()
        {
            var buyerId = HttpContext.Session.GetInt32("UserId");
            if (buyerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var inquiries = _context.Inquiries
                .Include(i => i.Product)
                    .ThenInclude(p => p.Farmer)
                .Where(i => i.BuyerId == buyerId)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

            return View(inquiries);
        }
    }
}
