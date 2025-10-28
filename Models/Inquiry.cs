using System.ComponentModel.DataAnnotations;

namespace FarmerMarket.Models
{
    public class Inquiry
    {
        public int InquiryId { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int BuyerId { get; set; }
        
        [Required]
        [Range(0.01, 999999)]
        public decimal RequiredQuantity { get; set; }
        
        [StringLength(500)]
        public string? Message { get; set; }
        
        public string Status { get; set; } = "Pending";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public Product? Product { get; set; }
        public User? Buyer { get; set; }
    }
}
