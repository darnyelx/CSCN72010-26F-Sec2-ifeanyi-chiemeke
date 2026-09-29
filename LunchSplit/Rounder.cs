using System;
using System.Collections.Generic;

namespace LunchSplit
{
    // Rounds each share to cents.
    public class Rounder
    {
        // Rounds every share. None leaves the raw decimals alone.
        public List<Share> RoundShares(List<Share> rawShares, RoundingMode mode)
        {
            List<Share> roundedShares = new List<Share>();

            for (int i = 0; i < rawShares.Count; i++)
            {
                Share share = rawShares[i];
                decimal roundedAmount = RoundAmount(share.Amount, mode);
                roundedShares.Add(new Share(share.Name, roundedAmount));
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
