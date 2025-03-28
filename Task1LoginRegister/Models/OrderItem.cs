using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Task1LoginRegister.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        // snapshot prices and taxes details as per unit at order time
        [Required]
        public decimal BasePrice { get; set; } 
        [Required]
        public decimal SnapshotPrice { get; set; }  
        [Required]
        public decimal SnapshotCostPrice { get; set; }  

        [Required]
        public decimal SnapshotProfitPercentage { get; set; }  
        [Required]
        public decimal SnapshotDiscountPercentage { get; set; }  
                                                                
        [Required]
        public decimal SnapshotCGSTPercentage { get; set; }  

        [Required]
        public decimal SnapshotSGSTPercentage { get; set; }  

        [Required]
        public decimal SnapshotGSTAmount { get; set; } 

        
        [Required]
        public decimal DeliveryCharge { get; set; } 

        public string ProductName { get; set; }
        public string ProductSKU { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalPrice { get; private set; }


        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
