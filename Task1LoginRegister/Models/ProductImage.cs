using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int? VariantId { get; set; }

        [ForeignKey("VariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }

        //  indicate if this image is for a variant
        public bool IsVariantImage { get; set; }=false;
        public bool IsPrimaryImage { get; set; }

        public DateTime CreatedAt { get; set; }=DateTime.Now;
    }
}
