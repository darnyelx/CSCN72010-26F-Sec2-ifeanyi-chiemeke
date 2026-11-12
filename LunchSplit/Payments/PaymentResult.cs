using System.Net;

namespace LunchSplit.Payments
{
    // Gateway-independent result returned after a payment attempt.
    public sealed class PaymentResult
    {
        public bool IsSuccess { get; init; }
        public string TransactionId { get; init; } // Filled in when Square creates the payment.
        public string ErrorCode { get; init; } // Gateway code, such as CARD_DECLINED.
        public string ErrorMessage { get; init; }
        public HttpStatusCode? StatusCode { get; init; } // Useful for validation and diagnostics.
    }
}
