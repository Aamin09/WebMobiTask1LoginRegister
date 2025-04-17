using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class VariantAttributeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VariantId { get; set; }
        [ForeignKey("VariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }

        [Required]
        public int AttributeValueId { get; set; }
        [ForeignKey("AttributeValueId")]
        public virtual ProductAttributeValue? ProductAttributeValue { get; set; }
    }
}
