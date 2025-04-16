using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class ProductAttributeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValueId { get; set; }

        [Required]
        public int AttributeId { get; set; }

        [ForeignKey("AttributeId")]
        public virtual ProductAttribute? Attribute { get; set; }

        [Required,StringLength(100)]
        public string Value { get; set; }

        [Required]
        public bool IsActive { get; set; }=true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<VariantAttributeValue>? VariantAttributeValues { get; set; }

    }
}
