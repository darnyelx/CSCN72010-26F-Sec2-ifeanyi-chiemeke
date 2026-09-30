using System;
using System.Collections.Generic;

namespace LunchSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Lunch Split!");
            Console.WriteLine();

            // Sample bill: $100.00, $13.00 tax, and a 15 percent tip.
            Bill bill = new Bill(100.00m, 13.00m, TipMode.Percent, 15.00m);

            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            attendees.Add(new Attendee("Ben", 1, true));

            Splitter splitter = new Splitter();
            List<Share> shares = splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);

            ReceiptFormatter formatter = new ReceiptFormatter();
            string receipt = formatter.Format(bill, attendees, shares);
            Console.WriteLine(receipt);

            Console.WriteLine("Thanks for using Lunch Split!");
        }
    }
}
