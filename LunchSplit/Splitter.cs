using System;
using System.Collections.Generic;

namespace LunchSplit
{
    // Calculates the tip and each person's share.
    public class Splitter
    {
        // Returns the tip. Percent uses a number like 15 for 15 percent.
        public decimal ComputeTip(decimal subtotal, TipMode mode, decimal tipInput)
        {
            if (subtotal < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(subtotal), "Subtotal cannot be negative.");
            }

            if (mode == TipMode.None)
            {
                return 0.00m;
            }

            if (mode == TipMode.Fixed)
            {
                if (tipInput < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(tipInput), "Fixed tip cannot be negative.");
                }

                return tipInput;
            }

            // Step 1: turn the percent into a fraction.
            decimal percent = tipInput / 100m;

            // Step 2: apply that fraction to the subtotal.
            decimal tip = subtotal * percent;

            // Step 3: keep the tip in cents.
            tip = Math.Round(tip, 2);
            return tip;
        }

        // Splits the grand total. An excluded person gets $0.00.
        public List<Share> CalculateShares(Bill bill, List<Attendee> attendees, RoundingMode roundingMode)
        {
            decimal tip = ComputeTip(bill.Subtotal, bill.TipMode, bill.TipInput);
            decimal grandTotal = bill.Subtotal + bill.Tax + tip;

            // Step 1: only included people with a weight count toward the split.
            int includedCount = 0;
            int totalWeight = 0;

            for (int i = 0; i < attendees.Count; i++)
            {
                Attendee person = attendees[i];
                if (person.Included)
                {
                    includedCount = includedCount + 1;
                    if (person.Weight > 0)
                    {
                        totalWeight = totalWeight + person.Weight;
                    }
                }
            }

            if (includedCount == 0)
            {
                throw new InvalidOperationException("At least one person must be included.");
            }

            if (totalWeight == 0)
            {
                throw new InvalidOperationException("Total weight cannot be zero.");
            }

            // Step 2: each included person gets their fraction of the total.
            List<Share> rawShares = new List<Share>();

            for (int i = 0; i < attendees.Count; i++)
            {
                Attendee person = attendees[i];
                decimal amount = 0.00m;

                if (person.Included && person.Weight > 0 && totalWeight > 0)
                {
                    amount = grandTotal * person.Weight / totalWeight;
                }

                rawShares.Add(new Share(person.Name, amount));
            }

            // Step 3: round the shares so they still add up.
            Rounder rounder = new Rounder();
            List<Share> roundedShares = rounder.RoundShares(rawShares, roundingMode);
            return roundedShares;
        }
    }
}
