using Razorpay.Api;
namespace Task1LoginRegister.Services
{
    public class RazorPayService
    {
        private readonly IConfiguration configuration;
        private readonly string _keyId;
        private readonly string _keySecret;
        RazorpayClient client;

        public RazorPayService(IConfiguration configuration)
        {
            this.configuration = configuration;
            _keyId = configuration["Razorpay:KeyId"];
            _keySecret = configuration["Razorpay:KeySecret"];
            client = new RazorpayClient(_keyId, _keySecret);

        }

        public Dictionary<string, object> CreateOrder(decimal amount, string receipt, string currency = "INR")
        {

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

        // Refund Service
        public Refund ProcessRefund(string paymentId, decimal amount=0, string notes=null)
        {
            Dictionary<string, object> refundRequest = new Dictionary<string, object>();

            // convert to paise
            if(amount > 0)
            {
                refundRequest.Add("amount", (int)(amount * 100));
            }

            if(!string.IsNullOrEmpty(notes))
            {
                Dictionary<string,string> notesDictionary= new Dictionary<string, string>
                {
                    {"reason",notes }
                };
                refundRequest.Add("notes", notesDictionary);
            }
            return client.Payment.Fetch(paymentId).Refund(refundRequest);
        }

        // Get refund details
        public Refund GetRefundDetails(string refundId)
        {
            return client.Refund.Fetch(refundId);   
        }

        // list all refund for a payment
        public List<Refund> GetRefundsForPayment(string paymentId)
        {
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                {"payment_id",paymentId }
            };

            List<Refund> refunds = client.Refund.All(options);
            return refunds;
        }
    }
}
