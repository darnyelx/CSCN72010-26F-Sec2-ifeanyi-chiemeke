using System;

namespace LunchSplit
{
    // This class splits a lunch bill
    public class BillSplitter
    {
        // Splits the total evenly between people
        public double SplitEvenly(double total, int people)
        {
            if (people <= 0)
            {
                return 0;
            }

            double amount = total / people;
            amount = Math.Round(amount, 2);
            return amount;
        }

        // Splits the total after adding a tip
        public double SplitWithTip(double total, double tipPercent, int people)
        {
            double tip = total * (tipPercent / 100);
            double grandTotal = total + tip;
            double amount = SplitEvenly(grandTotal, people);
            return amount;
        }
    }
}
