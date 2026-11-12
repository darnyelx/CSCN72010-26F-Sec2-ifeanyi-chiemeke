using System.Net;

namespace LunchSplit.Payments
{
    /// <summary>
    /// Contains the application-level outcome of a payment attempt.
    /// </summary>
    public sealed class PaymentResult
    {
        /// <summary>
        /// Gets whether the payment was accepted by the gateway.
        /// </summary>
        public bool IsSuccess { get; init; }

        /// <summary>
        /// Gets the gateway transaction identifier when the payment succeeds.
        /// </summary>
        public string TransactionId { get; init; }

        /// <summary>
        /// Gets the gateway error code when the payment fails, such as CARD_DECLINED.
        /// </summary>
        public string ErrorCode { get; init; }

        /// <summary>
        /// Gets a human-readable explanation of the payment failure.
        /// </summary>
        public string ErrorMessage { get; init; }

        /// <summary>
        /// Gets the HTTP status returned by the gateway, when one is available.
        /// </summary>
        public HttpStatusCode? StatusCode { get; init; }
    }
}
