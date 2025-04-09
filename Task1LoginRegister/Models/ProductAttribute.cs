using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class ProductAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributeId { get; set; }

        [Required,StringLength(100),DisplayName("Attribute Name")]
        public string Name { get; set; }

        [Required]
        public bool IsAcrive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ProductAttributeValue>? ProductAttributeValue { get; set; }

    }
}
