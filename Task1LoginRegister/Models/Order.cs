using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models
{
    public class Order
    {
        [Key]
        public int OrderId {  get; set; }
        [Required]
        public string OrderNumber { get; set; }

        [Required]
        public int UserId {  get; set; }
        [ForeignKey("UserId")]
        public virtual Userlogin? User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }= DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string OrderStatus { get; set; } = "Pending";

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; } = "Pending";

        public string? RazorpayOrderId { get; set; }

        public string? PaymentId { get; set; }
        [Required]
        public int DeliveryAddressId { get; set; }
        [ForeignKey("DeliveryAddressId")]
        public virtual DeliveryAddress? DeliveryAddress { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual RazorpayOrderModel? RazorpayOrder { get; set; }
        public virtual ICollection<RefundDetailsModel> RefundDetails { get; set; }
    }
}
