namespace LunchSplit.Payments
{
    public enum PaymentStatus
    {
        Unknown, // The gateway returned a status we do not recognize yet.
        Pending,
        Completed,
        Failed
    }
}
