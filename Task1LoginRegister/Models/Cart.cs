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

        [NotMapped]
        public decimal TotalGST
        {
            get
            {
                var taxes = Product?.Subcategory?.Taxes;
                if (taxes == null) return 0; 
                return ((Price * Quantity) * ((taxes.CGST + taxes.SGST) / 100));
            }
        }
    

    }
}
