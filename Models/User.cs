using System.ComponentModel.DataAnnotations;

namespace FarmerMarket.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [Phone]
        public string? Phone { get; set; }
        
        [Required]
        public string UserType { get; set; } = string.Empty;
        
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property
        public ICollection<Product>? Products { get; set; }
    }
}
