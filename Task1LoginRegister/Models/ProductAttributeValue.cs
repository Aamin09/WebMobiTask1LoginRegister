using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class ProductAttributeValue
    {
        [Key]
        [DisplayName("Value Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValueId { get; set; }

        [Required, DisplayName("Attribute Id")]
        public int AttributeId { get; set; }

        [ForeignKey("AttributeId")]
        public virtual ProductAttribute? Attribute { get; set; }

        [Required,StringLength(100)]
        public string Value { get; set; }

        [Required, DisplayName("Is Active")]
        public bool IsActive { get; set; }=true;
        [DisplayName("Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<VariantAttributeValue>? VariantAttributeValues { get; set; }
        public virtual ICollection<ProductAttributeValueMapping>? ProductAttributeValueMappings { get; set; }

    }
}
