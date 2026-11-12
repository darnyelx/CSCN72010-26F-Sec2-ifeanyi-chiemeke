using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunchSplit.Payments
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
            // Do not send a free bill to the payment provider.
            if (request.Amount == 0.00m)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorCode = "ZERO_AMOUNT",
                    ErrorMessage = "A zero-dollar payment does not require gateway processing."
                };
            }

            try
            {
                return await processor.ProcessPaymentAsync(request);
            }
            catch (HttpRequestException exception)
            {
                // Keep transport details out of the rest of the application.
                throw new PaymentGatewayTimeoutException(
                    "The payment gateway could not be reached.",
                    exception);
            }
        }

        public async Task<PaymentBatchResult> ProcessBatchAsync(
            IReadOnlyList<PaymentRequest> requests)
        {
            List<PaymentResult> results = new List<PaymentResult>();

            // Each attendee is independent, so one failure should not stop the remaining payments.
            for (int i = 0; i < requests.Count; i++)
            {
                try
                {
                    results.Add(await ProcessPaymentAsync(requests[i]));
                }
                catch (PaymentGatewayTimeoutException exception)
                {
                    results.Add(new PaymentResult
                    {
                        IsSuccess = false,
                        ErrorCode = "GATEWAY_TIMEOUT",
                        ErrorMessage = exception.Message
                    });
                }
            }

            return new PaymentBatchResult(results);
        }
    }
}
