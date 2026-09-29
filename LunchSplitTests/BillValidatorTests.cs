using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LunchSplit;

namespace LunchSplitTests
{
    [TestClass]
    public class BillValidatorTests
    {
        [TestMethod]
        public void Validate_CompleteBillDetails_ReturnsOk()
        {
            // Arrange
            BillValidator validator = new BillValidator();
            Bill bill = new Bill(40.00m, 5.20m, TipMode.Percent, 15m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));

            // Act
            ValidationResult result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_EmptyAttendeeCollection_ReturnsFail()
        {
            // Arrange
            BillValidator validator = new BillValidator();
            Bill bill = new Bill(40.00m, 5.20m, TipMode.None, 0m);
            List<Attendee> attendees = null;

            // Act
            ValidationResult result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Add at least one person.", result.Message);
        }

        [TestMethod]
        public void Validate_NegativeSubtotal_ReturnsFail()
        {
            // Arrange
            BillValidator validator = new BillValidator();
            Bill bill = new Bill(-1.00m, 0m, TipMode.None, 0m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));

            // Act
            ValidationResult result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Subtotal cannot be negative.", result.Message);
        }

        [TestMethod]
        public void Validate_NegativeTax_ReturnsFail()
        {
            // Arrange
            BillValidator validator = new BillValidator();
            Bill bill = new Bill(40.00m, -0.50m, TipMode.None, 0m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", 1, true));

            // Act
            ValidationResult result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax cannot be negative.", result.Message);
        }

        [TestMethod]
        public void Validate_ZeroAttendees_ReturnsFail()
        {
            // Arrange
            BillValidator validator = new BillValidator();
            Bill bill = new Bill(40.00m, 0m, TipMode.None, 0m);
            List<Attendee> attendees = new List<Attendee>();

            // Act
            ValidationResult result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Add at least one person.", result.Message);
        }

        [TestMethod]
        public void Validate_NegativeWeight_ReturnsFail()
        {
            // Arrange
            BillValidator validator = new BillValidator();
            Bill bill = new Bill(40.00m, 0m, TipMode.None, 0m);
            List<Attendee> attendees = new List<Attendee>();
            attendees.Add(new Attendee("Ana", -1, true));

            // Act
            ValidationResult result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Weight cannot be negative.", result.Message);
        }
    }
}
