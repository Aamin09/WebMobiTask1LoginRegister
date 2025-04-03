using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Userlogin User { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required, StringLength(500)]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
