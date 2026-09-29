using System;
using System.Collections.Generic;

namespace LunchSplit
{
    // Rounds each share to cents, then fixes leftover pennies.
    public class Rounder
    {
        // Rounds every share. None leaves the raw decimals alone.
        public List<Share> RoundShares(List<Share> rawShares, RoundingMode mode)
        {
            List<Share> roundedShares = new List<Share>();

            if (rawShares.Count == 0)
            {
                return roundedShares;
            }

            decimal rawTotal = 0;

            for (int i = 0; i < rawShares.Count; i++)
            {
                Share share = rawShares[i];
                rawTotal = rawTotal + share.Amount;
                decimal roundedAmount = RoundAmount(share.Amount, mode);
                roundedShares.Add(new Share(share.Name, roundedAmount));
            }

            if (mode == RoundingMode.None)
            {
                return roundedShares;
            }

            // Step 1: the rounded shares must add up to the rounded bill total.
            decimal target = RoundAmount(rawTotal, mode);
            decimal roundedTotal = 0;

            for (int i = 0; i < roundedShares.Count; i++)
            {
                roundedTotal = roundedTotal + roundedShares[i].Amount;
            }

            // Step 2: give or take one cent at a time, starting with the first person.
            decimal difference = target - roundedTotal;
            int index = 0;

            while (difference >= 0.01m || difference <= -0.01m)
            {
                decimal penny = 0.01m;
                if (difference < 0)
                {
                    penny = -0.01m;
                }

                Share current = roundedShares[index];
                decimal adjusted = current.Amount + penny;
                roundedShares[index] = new Share(current.Name, adjusted);
                difference = difference - penny;

                index = index + 1;
                if (index >= roundedShares.Count)
                {
                    index = 0;
                }
            }

            return roundedShares;
        }

        // Rounds one amount to the nearest cent, or leaves it unchanged.
        private decimal RoundAmount(decimal amount, RoundingMode mode)
        {
            if (mode == RoundingMode.None)
            {
                return amount;
            }

            if (mode == RoundingMode.Bankers)
            {
                // 10.325 stays 10.32 because 2 is even. 10.335 becomes 10.34.
                return Math.Round(amount, 2, MidpointRounding.ToEven);
            }

            if (mode == RoundingMode.RoundUp)
            {
                decimal cents = amount * 100m;
                cents = Math.Ceiling(cents);
                return cents / 100m;
            }

            // Round down drops any fraction of a cent.
            decimal downCents = amount * 100m;
            downCents = Math.Floor(downCents);
            return downCents / 100m;
        }
    }
}
