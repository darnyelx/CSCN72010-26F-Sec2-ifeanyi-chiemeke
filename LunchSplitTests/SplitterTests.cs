using System;
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
    }
}
