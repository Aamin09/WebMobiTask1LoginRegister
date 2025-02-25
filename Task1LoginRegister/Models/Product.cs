using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required]
        [StringLength(200)]
        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("SKU")]
        public string SKU { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [DisplayName("Price")]
        public decimal Price { get; set; }
        [Required]
        [Range(0,100)]
        [DisplayName("Discount (%)")]
        public decimal SellingPricePercent { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [DisplayName("Selling Price")]
        public decimal CalculatedSellingPrice { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        [DisplayName("Delivery Charge")]
        public decimal DeliveryCharge { get; set; } = 0;


        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [DisplayName("Subcategory")]
        public int SubcategoryId { get; set; }
        [ForeignKey("SubcategoryId")]
        public virtual Subcategory Subcategory { get; set; } 

        public virtual ICollection<ProductImage> ProductImages  { get; set; }   

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } 

    }
}
