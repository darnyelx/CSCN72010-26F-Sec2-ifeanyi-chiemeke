using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LunchSplit;

namespace LunchSplitTests
{
    [TestClass]
    public class ReceiptFormatterTests
    {
        [TestMethod]
        public void Format_DefaultBillReceipt_ContainsLineItems()
        {
            // Arrange
            ReceiptFormatter formatter = new ReceiptFormatter();
            Bill bill = new Bill(60.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            attendees.Add(new Attendee("Ben", 1, true));
            List<Share> shares = new List<Share>();
            shares.Add(new Share("Ana", 30.00m));
            shares.Add(new Share("Ben", 30.00m));

            // Act
            string receipt = formatter.Format(bill, attendees, shares);

            // Assert
            Assert.Contains("Ana: $30.00", receipt);
            Assert.Contains("Ben: $30.00", receipt);
        }

        [TestMethod]
        public void Format_ReceiptOutput_ContainsRequiredMetadata()
        {
            // Arrange
            ReceiptFormatter formatter = new ReceiptFormatter();
            Bill bill = new Bill(60.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            List<Share> shares = new List<Share>();
            shares.Add(new Share("Ana", 60.00m));

            // Act
            string receipt = formatter.Format(bill, attendees, shares);

            // Assert
            Assert.Contains("Ifeanyi Chiemeke", receipt);
            Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), receipt);
            Assert.Contains("Time:", receipt);
        }

        [TestMethod]
        public void Format_CsvReceiptExporter_MatchesSchema()
        {
            // Arrange
            // CSV export is a later feature, so this test stays red on purpose.

            // Act
            // There is no CSV exporter yet.

            // Assert
            Assert.Fail("CSV receipt export is not built yet.");
        }
    }
}
