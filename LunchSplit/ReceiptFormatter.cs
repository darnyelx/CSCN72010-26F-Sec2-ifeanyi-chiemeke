using System;
using System.Collections.Generic;

namespace LunchSplit
{
    // Turns a finished split into text that can be printed.
    public class ReceiptFormatter
    {
        // Builds one receipt with the student name, date, time, and each share.
        public string Format(Bill bill, List<Attendee> attendees, List<Share> roundedShares)
        {
            Splitter splitter = new Splitter();
            decimal tip = splitter.ComputeTip(bill.Subtotal, bill.TipMode, bill.TipInput);
            decimal total = bill.GrandTotal;
            DateTime created = DateTime.Now;

            string receipt = "";
            receipt = receipt + "LunchSplit Receipt" + "\n";
            receipt = receipt + "Student: Ifeanyi Chiemeke" + "\n";
            receipt = receipt + "Date: " + created.ToString("yyyy-MM-dd") + "\n";
            receipt = receipt + "Time: " + created.ToString("HH:mm:ss") + "\n";
            receipt = receipt + "\n";
            receipt = receipt + "Subtotal: $" + bill.Subtotal.ToString("0.00") + "\n";
            receipt = receipt + "Tax: $" + bill.Tax.ToString("0.00") + "\n";
            receipt = receipt + "Tip: $" + tip.ToString("0.00") + "\n";
            receipt = receipt + "Total: $" + total.ToString("0.00") + "\n";
            receipt = receipt + "\n";

            for (int i = 0; i < attendees.Count; i++)
            {
                Attendee person = attendees[i];
                decimal amount = 0.00m;

                for (int j = 0; j < roundedShares.Count; j++)
                {
                    if (roundedShares[j].Name == person.Name)
                    {
                        amount = roundedShares[j].Amount;
                    }
                }

                receipt = receipt + person.Name + ": $" + amount.ToString("0.00") + "\n";
            }

            return receipt;
        }
    }
}
