using System;

namespace LunchSplit.Payments
{
    // Gives callers one application-level error for a gateway that cannot be reached.
    public sealed class PaymentGatewayTimeoutException : Exception
    {
        public PaymentGatewayTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
