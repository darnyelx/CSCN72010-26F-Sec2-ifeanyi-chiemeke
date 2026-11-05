namespace LunchSplit
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

        public decimal Amount { get; }
        public string Currency { get; }
        public string IdempotencyKey { get; }
        public string CardNonce { get; }
    }
}
