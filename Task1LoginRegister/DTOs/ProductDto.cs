using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Task1LoginRegister.DTOs
{
    public class ProductDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(50, ErrorMessage = "Product name can't be longer than 50 characters.")]
        [DisplayName ("Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Short description is required.")]
        [StringLength(200, ErrorMessage = "Short description can't be longer than 200 characters.")]
        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }
        [DisplayName("Primary Image")]
        public IFormFile? PrimaryImage { get; set; }
        [DisplayName("Gallery Images")]
        public ICollection<IFormFile>? GalleryImages { get; set; } = new List<IFormFile>();

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [DisplayName("Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Selling price percentage is required.")]
        [Range(0, 100, ErrorMessage = "Selling price percentage must be between 0 and 100.")]
        [DisplayName("Selling Price Percentage")]
        public decimal SellingPricePercentage { get; set; }
        [DisplayName("Status")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Subcategory is required.")]
        [DisplayName("Subcategory")]
        public int SubcategoryId { get; set; }

        [Required]
        [DisplayName("Delivery Charge")]
        public decimal DeliveryCharge { get; set; } = 0;
        [Required, DisplayName("Stock")]

        public int StockQuantity { get; set; }
        [Required, DisplayName("Minimumn Stock Level")]

        public int MinimumStockLevel { get; set; } 
        public string? PrimaryImageUrl { get; set; }

        public List<ProductImageDto> ExistingGalleryImages { get; set; } = new List<ProductImageDto>();
        public List<int> ImagesToDelete { get; set; } = new List<int>();
        [DisplayName("SKU")]
        public string? SKU { get; set; }

        // Computed Property for Selling Price Calculation
        [DisplayName("Calculated Selling Price")]
        public decimal CalculatedSellingPrice => Price + (Price * SellingPricePercentage / 100);
    }
}
