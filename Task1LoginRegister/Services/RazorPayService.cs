using Razorpay.Api;
namespace Task1LoginRegister.Services
{
    public class RazorPayService
    {
        private readonly IConfiguration configuration;
        private readonly string _keyId;
        private readonly string _keySecret;


        public RazorPayService(IConfiguration configuration)
        {
            this.configuration = configuration;
            _keyId = configuration["Razorpay:KeyId"];
            _keySecret = configuration["Razorpay:KeySecret"];
        }

        public Dictionary<string, object> CreateOrder(decimal amount, string receipt, string currency = "INR")
        {
            RazorpayClient client= new RazorpayClient(_keyId,_keySecret);

            Dictionary<string, object> options = new Dictionary<string, object>()
            {
                {"amount",amount*100 }, // razorpay expect amount in paise
                {"currency",currency },
                {"receipt",receipt },
                {"payment_capture",1 } // Auto capture payment
            };

            Order order= client.Order.Create(options);
            return order.Attributes.ToObject<Dictionary<string, object>>();
        }

        public Payment GetPaymentById(string paymentId)
        {
            RazorpayClient client= new RazorpayClient(_keyId, _keySecret);  
            return client.Payment.Fetch(paymentId);
        }
    }
}
