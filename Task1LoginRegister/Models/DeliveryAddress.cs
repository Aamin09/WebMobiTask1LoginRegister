using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class DeliveryAddress
    {
        [Key]
        public int AddressId { get; set; }  
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Userlogin? User { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required, StringLength(100)]
        public string City { get; set; }

        [Required, StringLength(100)]
        public string State { get; set; }

        [Required,StringLength(6)]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]

        public string Email { get; set; }

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
