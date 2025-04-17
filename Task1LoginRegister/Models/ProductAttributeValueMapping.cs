using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task1LoginRegister.Models
{
    public class ProductAttributeValueMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        public int AttributeValueId { get; set; }
        [ForeignKey("AttributeValueId")]
        public virtual ProductAttributeValue? ProductAttributeValue { get; set; }
    }
}
