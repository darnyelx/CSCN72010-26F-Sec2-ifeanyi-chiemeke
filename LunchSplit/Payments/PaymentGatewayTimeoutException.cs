using System;

namespace LunchSplit.Payments
{
    public sealed class PaymentGatewayTimeoutException : Exception
    {
        public PaymentGatewayTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
