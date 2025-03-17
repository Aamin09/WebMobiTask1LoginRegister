using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task1LoginRegister.Models
{
    public class RefundDetailsModel
    {
        [Key]
        public string RefundId { get; set; }

        public string PaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public virtual Order? Order { get; set; }

        public string Notes { get; set; }

        // Speed code for easy access
        public string SpeedCode { get; set; }
    }
}
