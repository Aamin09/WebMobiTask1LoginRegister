using Task1LoginRegister.Models;

namespace Task1LoginRegister.DTOs
{
    public class CheckoutViewDto
    {
        public List<DeliveryAddress> DeliveryAddresses { get; set; }
        public DeliveryAddress NewDeliveryAddress { get; set; }

        public List<Cart> CartItems { get; set; }

        public int? SelectedAddressId { get; set; }
        public string PaymentMethod { get; set; }

    }
}
