using Microsoft.EntityFrameworkCore;

namespace FarmerMarket.Models
{
    public class FarmerMarketContext : DbContext
    {
        public FarmerMarketContext(DbContextOptions<FarmerMarketContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "Admin",
                    Email = "admin@farmermarket.com",
                    Password = "admin123",
                    UserType = "Admin",
                    Phone = "9999999999",
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    UserId = 2,
                    Name = "Ramesh Patel",
                    Email = "ramesh@gmail.com",
                    Password = "farmer123",
                    UserType = "Farmer",
                    Phone = "9876543210",
                    City = "Surat",
                    State = "Gujarat",
                    Address = "Village Road, Bardoli",
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    UserId = 3,
                    Name = "Suresh Kumar",
                    Email = "suresh@gmail.com",
                    Password = "buyer123",
                    UserType = "Buyer",
                    Phone = "9988776655",
                    City = "Surat",
                    State = "Gujarat",
                    CreatedAt = DateTime.Now
                }
            );
            
            // Seed sample products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    FarmerId = 2,
                    ProductName = "Fresh Tomatoes",
                    Category = "Vegetable",
                    Price = 40.00m,
                    Unit = "Kg",
                    AvailableQuantity = 100.00m,
                    Description = "Farm fresh red tomatoes",
                    IsOrganic = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    ProductId = 2,
                    FarmerId = 2,
                    ProductName = "Organic Wheat",
                    Category = "Grain",
                    Price = 2500.00m,
                    Unit = "Quintal",
                    AvailableQuantity = 10.00m,
                    Description = "Premium quality organic wheat",
                    IsOrganic = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    ProductId = 3,
                    FarmerId = 2,
                    ProductName = "Green Chillies",
                    Category = "Vegetable",
                    Price = 80.00m,
                    Unit = "Kg",
                    AvailableQuantity = 50.00m,
                    Description = "Fresh green chillies",
                    IsOrganic = false,
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}
