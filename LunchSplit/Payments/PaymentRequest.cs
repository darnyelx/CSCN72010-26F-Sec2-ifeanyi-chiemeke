namespace LunchSplit.Payments
{
    // Domain request sent to a payment gateway.
    public sealed class PaymentRequest
    {
        public PaymentRequest(decimal amount, string currency, string idempotencyKey, string cardNonce)
        {
            Amount = amount;
            Currency = currency;
            IdempotencyKey = idempotencyKey;
            CardNonce = cardNonce;
        }

        public decimal Amount { get; } // Amount in normal currency units, such as 12.50.
        public string Currency { get; } // ISO currency code expected by the gateway.
        public string IdempotencyKey { get; } // Lets retries reuse the same payment safely.
        public string CardNonce { get; } // Tokenized card source returned by Square's web SDK.
    }
}
