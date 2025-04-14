using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class ProductVariant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariantId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required, StringLength(100), DisplayName("Variant Name")]
        public string VarinatName { get; set; }
        [DisplayName("Variant Description")]
        public string? VarinatDescription { get; set; }

        [Required, StringLength(50), DisplayName("SKU")]
        public string SKU { get; set; }

        [Required, Range(0, double.MaxValue), DisplayName("Variant Cost Price")]
        public decimal CostPrice { get; set; }

        [Required,Range(0,1000),DisplayName("Profit (%)")]
        public decimal ProfitPercentage { get; set; }

        [Required,Range(0,double.MaxValue),DisplayName("Price")]
        public decimal BasePrice { get; set; } = 0;

        [Required, Range(0,100), DisplayName("Discount (%)")]
        public decimal DiscountPercentage { get; set; } = 0;
        [Required, Range(0, double.MaxValue), DisplayName("Final Selling Price")]
        public decimal FinalSellingPrice { get; set; } = 0;
        [Required, DisplayName("Stock")]
        public int StockQuantity { get; set; } = 0;
        [Required]
        public bool IsActive { get; set; }=true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public void CalculatePricing()
        {
            BasePrice = Math.Round(CostPrice * (1 + (ProfitPercentage / 100m)), 2);
            FinalSellingPrice=Math.Round(BasePrice * (1-(DiscountPercentage/100m)), 2);
        }

        public void InitializeFromBaseProductPrice(Product product)
        {
            CostPrice = product.CostPrice; 
            ProfitPercentage=product.ProfitPercentage;
            DiscountPercentage=product.SellingPricePercent;
            CalculatePricing();
        }
        public bool IsValidPricing()
        {
            return BasePrice > CostPrice && FinalSellingPrice > 0 && FinalSellingPrice <= BasePrice;
        }

    }
}
