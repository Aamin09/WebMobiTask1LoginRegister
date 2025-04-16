using Org.BouncyCastle.Security.Certificates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public int  UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Userlogin User { get; set; }
        [Required]
        [Range(0, 1000, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
        [NotMapped]
        public decimal TotalPrice=> (Price*Quantity) ;

        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public bool IsActive { get; set; } = true;
        public int? ProductVariantId { get; set; }
        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }
        
        public string? SelectedAttributes { get; set; }// Store as JSON or comma-separated values
        [NotMapped]
        public decimal TotalGST
        {
            get
            {
                var taxes = Product?.Subcategory?.Taxes;
                if (taxes == null) return 0; 

                decimal appicablePrice= ProductVariantId != null && ProductVariant != null 
                    ? ProductVariant.FinalSellingPrice 
                    :Price;

                return ((appicablePrice * Quantity) * ((taxes.CGST + taxes.SGST) / 100));
            }
        }
    

    }
}
