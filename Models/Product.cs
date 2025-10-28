using System.ComponentModel.DataAnnotations;

namespace FarmerMarket.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required]
        public int FarmerId { get; set; }
        
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        [Required]
        public string Unit { get; set; } = string.Empty;
        
        [Range(0, 999999)]
        public decimal? AvailableQuantity { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public string? ImagePath { get; set; }
        
        public bool IsOrganic { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property
        public User? Farmer { get; set; }
    }
}
