using System.Net;

namespace LunchSplit
{
    // Gateway-independent result returned after a payment attempt.
    public sealed class PaymentResult
    {
        public bool IsSuccess { get; init; }
        public string TransactionId { get; init; }
        public string ErrorCode { get; init; }
        public string ErrorMessage { get; init; }
        public HttpStatusCode? StatusCode { get; init; }
    }
}
