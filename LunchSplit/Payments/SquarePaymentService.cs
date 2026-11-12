using System;
using System.Linq;
using System.Threading.Tasks;
using Square;
using Square.Payments;

namespace LunchSplit.Payments
{
    // Infrastructure adapter that keeps Square SDK types outside the domain layer.
    public sealed class SquarePaymentService : IPaymentProcessor
    {
        private readonly SquareClient client;
        private readonly string locationId;

        public SquarePaymentService(string accessToken, string locationId)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("A Square access token is required.", nameof(accessToken));
            }

            if (string.IsNullOrWhiteSpace(locationId))
            {
                throw new ArgumentException("A Square location ID is required.", nameof(locationId));
            }

            this.locationId = locationId;
            // Sandbox is deliberate here; production credentials should use a separate service configuration.
            client = new SquareClient(
                accessToken,
                new ClientOptions
                {
                    BaseUrl = "https://connect.squareupsandbox.com",
                    Timeout = TimeSpan.FromSeconds(5),
                    MaxRetries = 0
                });
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                // Square expects the smallest currency unit, so dollars become cents here.
                Square.CreatePaymentResponse response = await client.Payments.CreateAsync(
                    new CreatePaymentRequest
                    {
                        SourceId = request.CardNonce,
                        IdempotencyKey = request.IdempotencyKey,
                        AmountMoney = new Money
                        {
                            Amount = checked((long)Math.Round(request.Amount * 100m, 0)),
                            Currency = Currency.FromCustom(request.Currency)
                        },
                        LocationId = locationId,
                        Autocomplete = true
                    });

                return new PaymentResult
                {
                    IsSuccess = true,
                    TransactionId = response.Payment?.Id
                };
            }
            catch (SquareApiException exception)
            {
                // Turn Square's error response into the application's gateway-neutral result.
                Square.Error error = exception.Errors?.FirstOrDefault();
                return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorCode = error == null ? null : error.Code.ToString(),
                    ErrorMessage = error?.Detail ?? exception.Message,
                    StatusCode = (System.Net.HttpStatusCode)exception.StatusCode
                };
            }
        }

        public async Task<PaymentStatus> GetTransactionStatusAsync(string transactionId)
        {
            // The domain only exposes the statuses it needs, rather than Square's SDK type.
            Square.GetPaymentResponse response = await client.Payments.GetAsync(
                new GetPaymentsRequest { PaymentId = transactionId });

            string status = response.Payment?.Status;
            return status switch
            {
                "COMPLETED" => PaymentStatus.Completed,
                "PENDING" => PaymentStatus.Pending,
                "APPROVED" => PaymentStatus.Pending,
                "FAILED" => PaymentStatus.Failed,
                _ => PaymentStatus.Unknown
            };
        }
    }
}
