using System.Net.Http;
using System.Threading.Tasks;

namespace LunchSplit
{
    // Coordinates payment use cases without exposing gateway-specific exceptions.
    public sealed class PaymentManager
    {
        private readonly IPaymentProcessor processor;

        public PaymentManager(IPaymentProcessor processor)
        {
            this.processor = processor;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                return await processor.ProcessPaymentAsync(request);
            }
            catch (HttpRequestException exception)
            {
                throw new PaymentGatewayTimeoutException(
                    "The payment gateway could not be reached.",
                    exception);
            }
        }
    }
}
