using System.Collections.Generic;

namespace LunchSplit
{
    // Checks a bill before anyone tries to split it.
    public class BillValidator
    {
        // Returns OK, or a short reason the bill cannot be split.
        public ValidationResult Validate(Bill bill, List<Attendee> attendees)
        {
            if (bill.Subtotal < 0)
            {
                return new ValidationResult(false, "Subtotal cannot be negative.");
            }

            if (bill.Tax < 0)
            {
                return new ValidationResult(false, "Tax cannot be negative.");
            }

            if (attendees == null || attendees.Count == 0)
            {
                return new ValidationResult(false, "Add at least one person.");
            }

            for (int i = 0; i < attendees.Count; i++)
            {
                if (attendees[i].Weight < 0)
                {
                    return new ValidationResult(false, "Weight cannot be negative.");
                }
            }

            return new ValidationResult(true, "Bill looks good.");
        }
    }
}
