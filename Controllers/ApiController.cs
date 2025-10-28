using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmerMarket.Models;

namespace FarmerMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly FarmerMarketContext _context;

        public ApiController(FarmerMarketContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Farmer)
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/products/category/Vegetable
        [HttpGet("products/category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _context.Products
                .Include(p => p.Farmer)
                .Where(p => p.Category == category)
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/farmers
        [HttpGet("farmers")]
        public async Task<ActionResult<IEnumerable<User>>> GetFarmers()
        {
            var farmers = await _context.Users
                .Where(u => u.UserType == "Farmer")
                .ToListAsync();

            return Ok(farmers);
        }

        // GET: api/stats
        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var stats = new
            {
                TotalFarmers = await _context.Users.CountAsync(u => u.UserType == "Farmer"),
                TotalBuyers = await _context.Users.CountAsync(u => u.UserType == "Buyer"),
                TotalProducts = await _context.Products.CountAsync(),
                TotalInquiries = await _context.Inquiries.CountAsync()
            };

            return Ok(stats);
        }
    }
}
