using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LunchSplit;

namespace LunchSplitTests
{
    [TestClass]
    public class SplitterTests
    {
        [TestMethod]
        public void ComputeTip_NoTipMode_ReturnsZero()
        {
            // Arrange
            Splitter splitter = new Splitter();

            // Act
            decimal tip = splitter.ComputeTip(50.00m, TipMode.None, 15.00m);

            // Assert
            Assert.AreEqual(0.00m, tip);
        }

        [TestMethod]
        public void ComputeTip_PercentTipMode_ReturnsCorrectPercent()
        {
            // Arrange
            Splitter splitter = new Splitter();

            // Act
            decimal tip = splitter.ComputeTip(120.00m, TipMode.Percent, 15.00m);

            // Assert
            Assert.AreEqual(18.00m, tip);
        }

        [TestMethod]
        public void ComputeTip_FixedTipMode_ReturnsFixedAmount()
        {
            // Arrange
            Splitter splitter = new Splitter();

            // Act
            decimal tip = splitter.ComputeTip(100.00m, TipMode.Fixed, 15.00m);

            // Assert
            Assert.AreEqual(15.00m, tip);
        }

        [TestMethod]
        public void ComputeTip_NegativeSubtotal_ThrowsException()
        {
            // Arrange
            Splitter splitter = new Splitter();

            // Act
            Action calculate = delegate ()
            {
                splitter.ComputeTip(-1.00m, TipMode.None, 0.00m);
            };

            // Assert
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(calculate);
        }

        [TestMethod]
        public void ComputeTip_NegativeFixedTip_ThrowsException()
        {
            // Arrange
            Splitter splitter = new Splitter();

            // Act
            Action calculate = delegate ()
            {
                splitter.ComputeTip(100.00m, TipMode.Fixed, -5.00m);
            };

            // Assert
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(calculate);
        }

        [TestMethod]
        public void CalculateShares_EqualSplit_ApportionsEvenly()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(90.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            attendees.Add(new Attendee("Ben", 1, true));
            attendees.Add(new Attendee("Cam", 1, true));

            // Act
            List<Share> shares = splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);

            // Assert
            Assert.AreEqual(30.00m, shares[0].Amount);
            Assert.AreEqual(30.00m, shares[1].Amount);
            Assert.AreEqual(30.00m, shares[2].Amount);
        }

        [TestMethod]
        public void CalculateShares_ProportionalSplit_ApportionsWeighted()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(90.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 2, true));
            attendees.Add(new Attendee("Ben", 1, true));

            // Act
            List<Share> shares = splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);

            // Assert
            Assert.AreEqual(60.00m, shares[0].Amount);
            Assert.AreEqual(30.00m, shares[1].Amount);
        }

        [TestMethod]
        public void CalculateShares_ExcludedAttendee_ApportionsZero()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(90.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            attendees.Add(new Attendee("Ben", 1, false));
            attendees.Add(new Attendee("Cam", 1, true));

            // Act
            List<Share> shares = splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);

            // Assert
            Assert.AreEqual(45.00m, shares[0].Amount);
            Assert.AreEqual(0.00m, shares[1].Amount);
            Assert.AreEqual(45.00m, shares[2].Amount);
        }

        [TestMethod]
        public void CalculateShares_TaxAndPercentTip_ApportionsTotal()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(100.00m, 13.00m, TipMode.Percent, 15.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            attendees.Add(new Attendee("Ben", 1, true));

            // Act
            List<Share> shares = splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);

            // Assert
            Assert.AreEqual(64.00m, shares[0].Amount);
            Assert.AreEqual(64.00m, shares[1].Amount);
            Assert.AreEqual(128.00m, shares[0].Amount + shares[1].Amount);
        }

        [TestMethod]
        public void CalculateShares_TaxAndFixedTip_ApportionsTotal()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(100.00m, 13.00m, TipMode.Fixed, 20.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));
            attendees.Add(new Attendee("Ben", 1, true));

            // Act
            List<Share> shares = splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);

            // Assert
            Assert.AreEqual(66.50m, shares[0].Amount);
            Assert.AreEqual(66.50m, shares[1].Amount);
            Assert.AreEqual(133.00m, shares[0].Amount + shares[1].Amount);
        }

        [TestMethod]
        public void CalculateShares_AllAttendeesExcluded_ThrowsException()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(90.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, false));
            attendees.Add(new Attendee("Ben", 1, false));

            // Act
            Action calculate = delegate ()
            {
                splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);
            };

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(calculate);
        }

        [TestMethod]
        public void CalculateShares_TotalWeightsZero_ThrowsException()
        {
            // Arrange
            Splitter splitter = new Splitter();
            Bill bill = new Bill(90.00m, 0.00m, TipMode.None, 0.00m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 0, true));
            attendees.Add(new Attendee("Ben", 0, true));

            // Act
            Action calculate = delegate ()
            {
                splitter.CalculateShares(bill, attendees, RoundingMode.Bankers);
            };

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(calculate);
        }
    }
}
