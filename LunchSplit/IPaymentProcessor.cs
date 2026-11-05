using System.Threading.Tasks;

namespace LunchSplit
{
    // Keeps payment orchestration independent from Square or another gateway.
    public interface IPaymentProcessor
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentStatus> GetTransactionStatusAsync(string transactionId);
    }
}
