using Microsoft.AspNetCore.Mvc;
using FarmerMarket.Models;
using System.Diagnostics;

namespace FarmerMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly FarmerMarketContext _context;

        public HomeController(FarmerMarketContext context)
        {
            _context = context;
        }

        // Practical 2 & 3: Display greeting based on time
        public IActionResult Index()
        {
            // Practical 3: Time-based greeting
            var currentHour = DateTime.Now.Hour;
            string greeting;
            string greetingImage;

            if (currentHour < 12)
            {
                greeting = "Good Morning!";
                greetingImage = "/images/morning.jpg";
            }
            else if (currentHour < 17)
            {
                greeting = "Good Afternoon!";
                greetingImage = "/images/afternoon.jpg";
            }
            else
            {
                greeting = "Good Evening!";
                greetingImage = "/images/evening.jpg";
            }

            // Practical 13: ViewBag to pass data to view
            ViewBag.Greeting = greeting;
            ViewBag.GreetingImage = greetingImage;
            ViewBag.TotalFarmers = _context.Users.Count(u => u.UserType == "Farmer");
            ViewBag.TotalProducts = _context.Products.Count();

            return View();
        }

        // Practical 2: Custom view method
        public ViewResult About()
        {
            return View("About");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
