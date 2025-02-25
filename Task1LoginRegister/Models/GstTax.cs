using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class GstTax
    {
        [Key]
        public int GSTId { get; set; }
        [Required]
        public int SubcategoryId { get; set; } 

        [ForeignKey("SubcategoryId")]
        public virtual Subcategory? Subcategory { get; set; }

        [Required]
        public decimal CGST { get; set; }  

        [Required]
        public decimal SGST { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        
        public DateTime? UpdatedAt {  get; set; }

    }
}
