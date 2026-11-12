using System.Threading.Tasks;

namespace LunchSplit.Payments
{
    // Keeps payment orchestration independent from Square or another gateway.
    public interface IPaymentProcessor
    {
        // Starts a payment without making the rest of the application know about Square.
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);

        // Looks up the gateway status after a payment has been created.
        Task<PaymentStatus> GetTransactionStatusAsync(string transactionId);
    }
}
