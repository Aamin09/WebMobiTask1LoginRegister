using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Task1LoginRegister.Models
{
    public class ProductAttributeValueMapping
    {
        [Key]
        [DisplayName("Mapping Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, DisplayName("Product Id")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required, DisplayName("Attribute Value Id")]
        public int AttributeValueId { get; set; }
        [ForeignKey("AttributeValueId")]
        public virtual ProductAttributeValue? ProductAttributeValue { get; set; }
    }
}
