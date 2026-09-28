using System;

namespace LunchSplit
{
    // Calculates the tip from a bill subtotal.
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
    }
}
