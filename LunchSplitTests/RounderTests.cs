using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LunchSplit;

namespace LunchSplitTests
{
    [TestClass]
    public class RounderTests
    {
        [TestMethod]
        public void RoundShares_NoRounding_PreservesRawDecimals()
        {
            // Arrange
            Rounder rounder = new Rounder();
            List<Share> rawShares = new List<Share>();
            rawShares.Add(new Share("Ana", 3.333m));

            // Act
            List<Share> roundedShares = rounder.RoundShares(rawShares, RoundingMode.None);

            // Assert
            Assert.AreEqual(3.333m, roundedShares[0].Amount);
        }

        [TestMethod]
        public void RoundShares_BankersRounding_RoundsToNearestEven()
        {
            // Arrange
            Rounder rounder = new Rounder();
            List<Share> rawShares = new List<Share>();
            rawShares.Add(new Share("Ana", 10.325m));
            rawShares.Add(new Share("Ben", 10.335m));

            // Act
            List<Share> roundedShares = rounder.RoundShares(rawShares, RoundingMode.Bankers);

            // Assert
            Assert.AreEqual(10.32m, roundedShares[0].Amount);
            Assert.AreEqual(10.34m, roundedShares[1].Amount);
        }

        [TestMethod]
        public void RoundShares_RoundUp_AppliesCeilingToCents()
        {
            // Arrange
            Rounder rounder = new Rounder();
            List<Share> rawShares = new List<Share>();
            rawShares.Add(new Share("Ana", 10.331m));

            // Act
            List<Share> roundedShares = rounder.RoundShares(rawShares, RoundingMode.RoundUp);

            // Assert
            Assert.AreEqual(10.34m, roundedShares[0].Amount);
        }

        [TestMethod]
        public void RoundShares_RoundDown_AppliesFloorToCents()
        {
            // Arrange
            Rounder rounder = new Rounder();
            List<Share> rawShares = new List<Share>();
            rawShares.Add(new Share("Ana", 10.339m));

            // Act
            List<Share> roundedShares = rounder.RoundShares(rawShares, RoundingMode.RoundDown);

            // Assert
            Assert.AreEqual(10.33m, roundedShares[0].Amount);
        }

        [TestMethod]
        public void RoundShares_EmptyShareCollection_ReturnsEmpty()
        {
            // Arrange
            Rounder rounder = new Rounder();
            List<Share> rawShares = new List<Share>();

            // Act
            List<Share> roundedShares = rounder.RoundShares(rawShares, RoundingMode.Bankers);

            // Assert
            Assert.IsEmpty(roundedShares);
        }
    }
}
