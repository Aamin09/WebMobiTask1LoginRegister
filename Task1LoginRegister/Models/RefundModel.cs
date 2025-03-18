using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Task1LoginRegister.Models
{
    public class RefundModel
    {
        public int OrderId { get; set; }
        public string PaymentId { get; set; }
        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage ="Amount must be greater than 0")]
        [DisplayName("Refund Amount")]
        public decimal RefundAmount {  get; set; }

        [Display(Name = "Reason for Refund")]
        public string RefundReason { get; set; }

    }
}
