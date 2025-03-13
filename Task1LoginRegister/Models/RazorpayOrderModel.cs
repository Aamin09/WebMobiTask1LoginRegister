using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task1LoginRegister.Models
{
    public class RazorpayOrderModel
    {
        [Key] // Mark OrderId as the Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OrderId { get; set; } // razorpay order id
        public string Razorpaykey { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public  string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
     
        [Required]
        [ForeignKey("Order")]
        public int ApplicationOrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
