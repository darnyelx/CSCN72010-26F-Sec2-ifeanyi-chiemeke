using System.Collections.Generic;

namespace LunchSplit.Payments
{
    // Summary of independent payment attempts in one attendee batch.
    public sealed class PaymentBatchResult
    {
        public PaymentBatchResult(IReadOnlyList<PaymentResult> results)
        {
            Results = results;
            SuccessCount = 0;
            FailureCount = 0;

            // Count from the returned results so the summary cannot drift from the details.
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].IsSuccess)
                {
                    SuccessCount++;
                }
                else
                {
                    FailureCount++;
                }
            }
        }

        public IReadOnlyList<PaymentResult> Results { get; }
        // These counts make it easy for the caller to show a batch summary.
        public int SuccessCount { get; }
        public int FailureCount { get; }
    }
}
