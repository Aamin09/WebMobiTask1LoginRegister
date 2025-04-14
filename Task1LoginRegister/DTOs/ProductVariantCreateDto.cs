using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Task1LoginRegister.Models;

namespace Task1LoginRegister.DTOs
{
    public class ProductVariantCreateDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<ProductVariantDto> Variants { get; set; }
        [ValidateNever]
        public IEnumerable<ProductAttribute> AvailableAttributes { get; set; }
    }

    public class ProductVariantEditDto
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public ProductVariantDto Variant { get; set; }
        [ValidateNever]
        public IEnumerable<ProductAttribute> AvailableAttributes { get; set; }
        [ValidateNever]
        public IEnumerable<ProductImage> ExistingImages { get; set; }
    }
}
