using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FarmerMarket.Models;
using FarmerMarket.Services;

namespace FarmerMarket.Controllers
{
    public class FarmerController : Controller
    {
        private readonly FarmerMarketContext _context;
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _environment;

        public FarmerController(FarmerMarketContext context, IMemoryCache cache, IWebHostEnvironment environment)
        {
            _context = context;
            _cache = cache;
            _environment = environment;
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            var farmerId = HttpContext.Session.GetInt32("UserId");
            if (farmerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Practical 10: Memory Cache
            string cacheKey = $"FarmerProducts_{farmerId}";
            if (!_cache.TryGetValue(cacheKey, out List<Product> products))
            {
                products = _context.Products
                    .Where(p => p.FarmerId == farmerId)
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, products, cacheOptions);
            }

            // Practical 13: ViewData
            ViewData["FarmerName"] = HttpContext.Session.GetString("UserName");
            ViewData["TotalProducts"] = products.Count;
            ViewData["TotalValue"] = products.Sum(p => p.CalculateTotalValue());

            return View(products);
        }

        // GET: Add Product (Practical 15)
        public IActionResult AddProduct()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        // POST: Add Product (Practical 8, 14, 15)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? imageFile)
        {
            var farmerId = HttpContext.Session.GetInt32("UserId");
            if (farmerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                product.FarmerId = farmerId.Value;
                product.CreatedAt = DateTime.Now;

                // Practical 8: Async file upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    product.ImagePath = "/images/products/" + uniqueFileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Clear cache
                _cache.Remove($"FarmerProducts_{farmerId}");

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("Dashboard");
            }

            return View(product);
        }

        // GET: Edit Product (Practical 15)
        public IActionResult EditProduct(int id)
        {
            var farmerId = HttpContext.Session.GetInt32("UserId");
            if (farmerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id && p.FarmerId == farmerId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Edit Product (Practical 15)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product, IFormFile? imageFile)
        {
            var farmerId = HttpContext.Session.GetInt32("UserId");
            if (farmerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                // Handle image upload if new file provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    product.ImagePath = "/images/products/" + uniqueFileName;
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                _cache.Remove($"FarmerProducts_{farmerId}");

                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Dashboard");
            }

            return View(product);
        }

        // Delete Product (Practical 15)
        public IActionResult DeleteProduct(int id)
        {
            var farmerId = HttpContext.Session.GetInt32("UserId");
            if (farmerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id && p.FarmerId == farmerId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                _cache.Remove($"FarmerProducts_{farmerId}");

                TempData["SuccessMessage"] = "Product deleted successfully!";
            }

            return RedirectToAction("Dashboard");
        }

        // View Inquiries
        public IActionResult Inquiries()
        {
            var farmerId = HttpContext.Session.GetInt32("UserId");
            if (farmerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var inquiries = _context.Inquiries
                .Include(i => i.Product)
                .Include(i => i.Buyer)
                .Where(i => i.Product.FarmerId == farmerId)
                .OrderByDescending(i => i.CreatedAt)
                .ToList();

            return View(inquiries);
        }
    }
}
