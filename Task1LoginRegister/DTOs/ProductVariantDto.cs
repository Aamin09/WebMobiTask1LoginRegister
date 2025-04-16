using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.DTOs
{
    public class ProductVariantDto
    {
        [Required, StringLength(100), DisplayName("Variant Name")]
        public string VarinatName { get; set; }
        [DisplayName("Variant Description")]
        public string? VarinatDescription { get; set; } = null;

        [Required, Range(0, double.MaxValue), DisplayName("Variant Cost Price")]
        public decimal CostPrice { get; set; }

        [Required, Range(0, 1000), DisplayName("Profit (%)")]
        public decimal ProfitPercentage { get; set; }

        [Required, Range(0, double.MaxValue), DisplayName("Price")]
        public decimal BasePrice { get; set; } = 0;

        [Required, Range(0, 100), DisplayName("Discount (%)")]
        public decimal DiscountPercentage { get; set; } = 0;
        [Required, Range(0, double.MaxValue), DisplayName("Final Selling Price")]
        public decimal FinalSellingPrice { get; set; } = 0;
        [Required, DisplayName("Stock")]
        public int StockQuantity { get; set; } = 0;
        [Required]
        public bool IsActive { get; set; } = true;

        public List<int> AttributeValueIds { get; set; } = new List<int>();
        public IFormFile? VarinatPrimaryImage { get; set; }
        public ICollection<IFormFile>? VarinatGalleryImages { get; set; }
        public List<int>? ImagesToDelete { get; set; }
        public void CalculatePricing()
        {
            BasePrice = Math.Round(CostPrice * (1 + (ProfitPercentage / 100m)), 2);
            FinalSellingPrice = Math.Round(BasePrice * (1 - (DiscountPercentage / 100m)), 2);
        }

        public void InitializeFromBaseProductPrice(Product product)
        {
            CostPrice = product.CostPrice;
            ProfitPercentage = product.ProfitPercentage;
            DiscountPercentage = product.SellingPricePercent;
            CalculatePricing();
        }
        public bool IsValidPricing()
        {
            return BasePrice > CostPrice && FinalSellingPrice > 0 && FinalSellingPrice <= BasePrice;
        }


    }


}
