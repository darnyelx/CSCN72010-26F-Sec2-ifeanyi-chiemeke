using System;
using System.Threading.Tasks;
using LunchSplit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LunchSplitTests
{
    [TestClass]
    public class SquareSandboxTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("M3-01")]
        public async Task ProcessPaymentAsync_WithSuccessfulMagicNonce_ReturnsTransactionId()
        {
            SquarePaymentService service = CreateConfiguredService();

            PaymentResult result = await service.ProcessPaymentAsync(new PaymentRequest(
                1.00m,
                "CAD",
                Guid.NewGuid().ToString(),
                "cnon:card-nonce-ok"));

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.TransactionId));
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("M3-02")]
        public async Task ProcessPaymentAsync_WithDeclinedMagicNonce_ReturnsCardDeclined()
        {
            SquarePaymentService service = CreateConfiguredService();

            PaymentResult result = await service.ProcessPaymentAsync(new PaymentRequest(
                1.00m,
                "CAD",
                Guid.NewGuid().ToString(),
                "cnon:card-nonce-declined"));

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("CARD_DECLINED", result.ErrorCode);
        }

        private static SquarePaymentService CreateConfiguredService()
        {
            string accessToken = Environment.GetEnvironmentVariable("SQUARE_SANDBOX_ACCESS_TOKEN");
            string locationId = Environment.GetEnvironmentVariable("SQUARE_SANDBOX_LOCATION_ID");

            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(locationId))
            {
                Assert.Inconclusive(
                    "Set SQUARE_SANDBOX_ACCESS_TOKEN and SQUARE_SANDBOX_LOCATION_ID to run Square Sandbox tests.");
            }

            return new SquarePaymentService(accessToken, locationId);
        }
    }
}
