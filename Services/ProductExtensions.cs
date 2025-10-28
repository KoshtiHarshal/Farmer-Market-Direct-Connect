using FarmerMarket.Models;

namespace FarmerMarket.Services
{
    public static class ProductExtensions
    {
        // Practical 7: Extension method to calculate total value
        public static decimal CalculateTotalValue(this Product product)
        {
            return product.Price * (product.AvailableQuantity ?? 0);
        }
        
        // Extension method to check if product is low stock
        public static bool IsLowStock(this Product product, decimal threshold = 10)
        {
            return (product.AvailableQuantity ?? 0) < threshold;
        }
        
        // Extension method to get discounted price
        public static decimal GetDiscountedPrice(this Product product, decimal discountPercent)
        {
            return product.Price - (product.Price * discountPercent / 100);
        }
        
        // Extension method to format price with unit
        public static string GetFormattedPrice(this Product product)
        {
            return $"₹{product.Price:N2} per {product.Unit}";
        }
    }
}
