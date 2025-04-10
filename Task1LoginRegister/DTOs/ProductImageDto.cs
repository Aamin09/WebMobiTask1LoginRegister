namespace Task1LoginRegister.DTOs
{
    public class ProductImageDto
    {
        public int ProductImageId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimaryImage { get; set; }
        public int? VariantId { get; set; }
        public bool IsVariantImage { get; set; }
    }
}
