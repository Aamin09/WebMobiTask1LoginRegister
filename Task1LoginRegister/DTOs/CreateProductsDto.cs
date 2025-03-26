using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Task1LoginRegister.DTOs
{
    public class CreateProductsDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(50, ErrorMessage = "Product name can't be longer than 50 characters.")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Short description is required.")]
        [StringLength(200, ErrorMessage = "Short description can't be longer than 200 characters.")]
        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [DisplayName("Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Selling price percentage is required.")]
        [Range(0, 100, ErrorMessage = "Selling price percentage must be between 0 and 100.")]
        [DisplayName("Selling Price Percentage")]
        public decimal SellingPricePercentage { get; set; }

        [DisplayName("Primary Image")]
        [Required]
        public IFormFile PrimaryImage { get; set; }

        [Required]
        [DisplayName("Gallery Images")]
        public ICollection<IFormFile> GalleryImages { get; set; }
        [Required]
        [DisplayName("Delivery Charge")]
        public decimal DeliveryCharge { get; set; } = 0;
        [Required, DisplayName("Stock")]
        public int StockQuantity { get; set; }
        [Required, DisplayName("Minimumn Stock Level")]
        public int MinimumStockLevel { get; set; } 

        [Required(ErrorMessage = "Category is required.")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Subcategory is required.")]
        [DisplayName("Subcategory")]
        public int SubcategoryId { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }
    }
}
